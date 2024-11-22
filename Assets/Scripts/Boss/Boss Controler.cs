using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControler : MonoBehaviour
{
    
    [Header("Debug")]
    public bool testAttack1Sequence;
    public bool testEnergyBall;
    public bool testAttack2Track;
    public bool testAttack2Lock;
    public bool testAttack2Sequence;
    
    [Header("Attributes")]
    public float trackSpeed = 1f;
    public float trackDistance = 15f;
    public int currentHP;
    public int maxHP;
    [Header("Idle")]
    public float idleWaitTime = 5f;
    private float idleTimer = 5f;
    [Header("Attack 1")]
    public int attack1TotalAttacks = 10;
    public float attack1Interval = 0.5f;
    private int attackCount = 0;
    private bool isAttacking1 = false;
    [Header("Attack 2")]    
    public float attack2Delay = 1f;
    public float attack2Timer = 0f;
    public float handTrackSpeed = 30f;
    public float handAttackSpeed = 40f;
    private int currentAttackingHand = 0;
    private Vector3 lockedPosition;
    private Attack2SubState attack2SubState = Attack2SubState.Still;
    public enum Attack2SubState{Tracking, Locked, Attacking, Still, Reset};
    [Header("State Params")]
    public bool isActive;
    public State state;
    public enum State{Idle, Attack1, Attack2, Dying};
    private bool dying = false;

    [Header("Internal References")]
    public GameObject head;
    public GameObject[] hands;    
    public Animator headAnimator;
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    [Header("Room limits")]
    public Transform leftBounds;
    public Transform rightBounds;
    public Transform topBounds;
    public Transform bottomBounds;
    
    [Header("External References")]
    public GameObject energyBallPrefab;
    public Transform energyBallSpawnPoint;    
    public GameObject endGameUI;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");   
        currentHP = maxHP;     
    }    
    void Update()
    {
        if(isActive){
            idleTimer -= Time.deltaTime;
            stateMachineController();
        }

        if(attackCount >= attack1TotalAttacks){
            isAttacking1 = false;
            CancelInvoke("Attack1SpawnMissile");
        }

        if(currentHP <= 0 && state != State.Dying){
            Die();
        }

        testMethods();
    }

    void FixedUpdate(){
        if(isActive && state != State.Idle && state != State.Dying){
            TrackPlayer();
        }
    }

    private void stateMachineController(){
        switch(state){
            case State.Idle:
                IdleBehaviour();                
                break;
            case State.Attack1:
                StartAttack1();
                break;
            case State.Attack2:                
                StartAttack2();
                break;
            case State.Dying:
                break;
        }
    }

    private void IdleBehaviour(){        
        SetIdleAnims();
        if(idleTimer<=0f){
            PickNextMove();
        }
    }
    private void StartAttack1(){        
        if(!isAttacking1){
            SetIdleAnims();
            headAnimator.Play("Attack1");
            isAttacking1 = true;
            attackCount = 0;
            InvokeRepeating("Attack1SpawnMissile", 0f, attack1Interval);
        }
        if(attackCount >= attack1TotalAttacks){
            isAttacking1 = false;
            state = State.Idle;
            idleTimer = idleWaitTime;
            CancelInvoke("Attack1SpawnMissile");
        }
        
    }

    private void Attack1SpawnMissile(){
        Instantiate(energyBallPrefab, energyBallSpawnPoint.position, Quaternion.identity);  
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.BOSSMISSILESPAWN);      
        attackCount++;
    }
    private void StartAttack2(){ 
        
        if(state == State.Attack2){
            switch(attack2SubState){
                case Attack2SubState.Reset:
                    idleTimer = idleWaitTime;
                    state = State.Idle;
                    return;
                case Attack2SubState.Still:  
                    currentAttackingHand = 0;              
                    detachHands(true, currentAttackingHand);         
                    attack2SubState = Attack2SubState.Tracking;
                    break;
                case Attack2SubState.Tracking:
                    att2MoveToPlayer(hands[currentAttackingHand].transform, currentAttackingHand);
                    break;
                case Attack2SubState.Locked:
                    WaitForAtt2();
                    break;
                case Attack2SubState.Attacking:
                    if(att2Thrust(hands[currentAttackingHand].transform, currentAttackingHand)){
                        idleTimer = idleWaitTime;
                        state = State.Idle;
                        return;
                    }

                    break;
            }        
        }
        

    }
    private void att2MoveToPlayer(Transform hand, int currentHand){
        Vector3 target = player.transform.position;
        if(currentHand == 0){
            target += new Vector3(5f, 0f, 0f);
        }else{
            target += new Vector3(-5f, 0f, 0f);
        }
        Vector3 handToPlayer = target - hand.position;

        if(handToPlayer.magnitude < 1f){
            Debug.Log("Locked in");
            lockedPosition = hand.position;
            attack2SubState = Attack2SubState.Locked;       
            attack2Timer = attack2Delay;     
            return;
        }
        hand.position += handToPlayer.normalized * Time.deltaTime * handTrackSpeed;
    }

    void WaitForAtt2(){
        attack2Timer -= Time.deltaTime;
        if(attack2Timer<=0f){
            Debug.Log("Starting Attack");
            attack2SubState = Attack2SubState.Attacking;
            AudioManagerBehaviour.PlayEnemySound(EnemySoundType.BOSSMELEEATTACK);
        }
    }

    bool att2Thrust(Transform hand, int currentHand){        
        Vector3 target = lockedPosition;
        if(currentHand == 0){
            target += new Vector3(-15f, 0f, 0f);
        }else{
            target += new Vector3(15f, 0f, 0f);
        }
        Vector3 handToTarget = target - hand.position;

        if(handToTarget.magnitude < 0.5){
            detachHands(false, currentAttackingHand);
            if(currentHand == 0){
                currentAttackingHand = 1;
                detachHands(true, currentAttackingHand);
                attack2SubState = Attack2SubState.Tracking;
            }else{
                detachHands(false, currentAttackingHand);
                idleTimer = idleWaitTime;
                attack2SubState = Attack2SubState.Reset;
                return true;
            }
        }

        hand.position += handToTarget.normalized * handAttackSpeed * Time.deltaTime;
        return false;
    }

    private void PickNextMove(){
        int nextMove = Random.Range(0,2);
        switch(nextMove){
            case 0:
                state = State.Attack1;
                break;
            case 1:
                state = State.Attack2;
                currentAttackingHand = 0;
                attack2SubState = Attack2SubState.Still;
                break;
        }
    }
    private void SetIdleAnims(){
        headAnimator.Play("HeadIdle");
        leftHandAnimator.Play("HandIdle");
        rightHandAnimator.Play("HandIdle");
    }

    public void detachHands(bool detach, int index){
        hands[index].GetComponent<followParent>().isDetached = detach;        
    }    

    private void TrackPlayer(){
        Vector3 target = player.transform.position + Vector3.up * 10f;
        if(head.transform.position.y > bottomBounds.position.y){
            Vector3 verticalVector = new Vector3(0f, target.y - head.transform.position.y, 0f);
            head.transform.position += verticalVector.normalized * trackSpeed * Time.deltaTime;
        }
        if(head.transform.position.y < topBounds.position.y){
            Vector3 verticalVector = new Vector3(0f, target.y - head.transform.position.y, 0f);
            head.transform.position += verticalVector.normalized * trackSpeed * Time.deltaTime;
        }        
        if(head.transform.position.x > leftBounds.position.x && head.transform.position.x < rightBounds.position.x){
            Vector3 horizontalVector = new Vector3(target.x - head.transform.position.x, 0f, 0f);
            head.transform.position += horizontalVector.normalized * trackSpeed/10 * Time.deltaTime;            
        }

        /* Vector3 posToTarget = player.transform.position - head.transform.position + Vector3.up * 10f;
        
        if (posToTarget.magnitude > trackDistance){
            head.transform.position += posToTarget * trackSpeed * Time.deltaTime / posToTarget.magnitude;
        }             */
    }

    void testMethods(){
        if(testAttack1Sequence){
            testAttack1Sequence = false;
            StartAttack1();            
        }

        if(testEnergyBall){
            testEnergyBall = false;
            Attack1SpawnMissile();
        }
    }

    private void Die(){
        if(!dying) AudioManagerBehaviour.PlayEnemySound(EnemySoundType.BOSSDEATH);
        dying = true;        
        GameObject.Find("MainCamera").GetComponent<CameraTracking>().target = head.transform;
        PlayDeathAnims();
        state = State.Dying;
        Debug.Log("Boss is dead");
    }

    public void Kill(){
        endGameUI.SetActive(true);
        Destroy(gameObject);
    }

    private void PlayDeathAnims(){
        headAnimator.Play("HeadDeath");
        leftHandAnimator.Play("HandKill");
        rightHandAnimator.Play("HandKill");
    }

    public void IsHurt(int damage = 1){
        Debug.Log("Boss damaged");
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if(currentHP <= 0 && state != State.Dying){
            Die();
        }
    }
}
