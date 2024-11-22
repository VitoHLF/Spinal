using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{
    [Header("Physics Parameters")]
    public float gravityScale = 1.0f;
    public float globalGravity = -10.0f; 
    public float speed = 1;
    public float flipSpeed = 500f;
    private bool isGrounded = false;
    public int direction = 1; // Direcao para a qual o inimigo esta virado
    public bool flipOnStart = false;
    public bool isGiant = false;

    [Header("Stats")]
    public float attackRange = 3f;
    public int HP = 4;
    

    [Header("References")]

    public Rigidbody rb;
    public GameObject sensor;
    public SpriteRenderer sr;
    public State state = State.Idle;
    public enum State{Idle, Walking, Attacking, Dying, Falling};

    public float sensorXLeft = -0.2f, sensorXRight = 1.6f; // Posicoes dos sensores de chao
    private GameObject player; // Alvo de perseguicao (jogador)
    public Animator animator;
    private bool isAttacking = false, isHurt = false, isDead = false;
    public float groundCheckBuffer=0.2f, lastGroundCheck=0f;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();        
        if(flipOnStart) Flip();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StateController();
        defaultPhysicsUpdate();
        lastGroundCheck -= Time.deltaTime;
        if(CheckWalls()) Flip();
    }

    void Update(){
        if(HP <= 0){
            state = State.Dying;
            isHurt = false;
            isAttacking = false;
            rb.velocity = Vector3.zero;            
        }
        fixRotation();
    }

    void defaultPhysicsUpdate(){
        if(!isGrounded){ 
            Vector3 gravity = globalGravity * gravityScale * Vector3.up; // Gravidade
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        /* if(state != State.Walking){
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        } */
    }

    public void Walk(){
        if(CheckGround()){ // Se houver chao ele se movimenta, caso contrario vira para a outra direcao
            Vector3 velocity = new Vector3(speed*direction, 0, 0);
            rb.velocity = velocity;
        }else{
            //Debug.Log("No ground below");
            Flip();
        }
    }

    public void Attack(){        
    }    

    public void Die(){
        Destroy(gameObject);
    }

    private bool CheckGround(){
        RaycastHit hit;
        if(Physics.Raycast(sensor.transform.position, Vector3.down, out hit, 2f)){ //Checagem se ha chao no caminho
            if(hit.transform.CompareTag("Ground")) {
                lastGroundCheck = groundCheckBuffer;
                return true;
            }
        }                
        if(lastGroundCheck>0f) return true;
        return false;
    }
    private bool CheckWalls(){
        Vector3 lookDirection = new Vector3(direction, 0, 0);
        RaycastHit hit;
        if(Physics.Raycast(sensor.transform.position, lookDirection, out hit, 0.5f)){ //Checagem se ha parede a frente
            if(hit.transform.CompareTag("Ground") || hit.transform.CompareTag("InvisibleWall")) return true;
        }                
        return false;
    }

    private bool CheckPlayer(){
        Vector3 lookDirection = new Vector3(direction, 0, 0);
        Vector3 thisPosition = new Vector3(transform.position.x, sensor.transform.position.y, transform.position.z);
        int layerMask = 1 << 3;
        RaycastHit hit;
        if (Physics.Raycast(thisPosition, lookDirection, out hit, attackRange, layerMask)) // Procura player a frente do objeto
        {           
            return true;
        }

        return false;
    }


    public void StateController(){     
        if(isHurt && !isAttacking) return;
        switch(state){
            case State.Dying:
                rb.velocity = Vector3.zero;
                isDead = true;
                animator.Play("Death");                
                break;
            case State.Idle:
                if(CheckPlayer()){
                    state = State.Attacking;
                    break;
                } 
                animator.Play("Idle");
                break;
            case State.Walking:
                if(CheckPlayer()){
                    state = State.Attacking;
                    break;
                } 
                animator.Play("Walking");                
                Walk();
                break;
            case State.Attacking:
                
                if(!CheckPlayer() && !isAttacking){
                    state = State.Walking;                
                    break;
                } 
                isAttacking = true;
                rb.velocity = Vector3.zero;
                animator.Play("Attacking");                
                break;            
            case State.Falling:
                if(CheckPlayer()){
                    state = State.Attacking;
                    break;
                } 
                Walk();
                break;
        }
    }

    public void Flip(){ // Altera a direcao que o objeto esta virado
        direction *= -1;    
        //sr.flipX = !sr.flipX;
        if(direction>0) sensor.transform.position = new Vector3(transform.position.x + sensorXRight, sensor.transform.position.y,sensor.transform.position.z);
        else sensor.transform.position = new Vector3(transform.position.x + sensorXLeft, sensor.transform.position.y,sensor.transform.position.z);
        
    }

    public void IsHurt(int damage = 1){
        HP -= damage;
        if(player.transform.position.x < transform.position.x) direction = -1;
        else direction = 1;
        if(HP <= 0){
            state = State.Dying;
            isHurt = false;
            isAttacking = false;
            rb.velocity = Vector3.zero;
            return;
        }
        if((isGiant && !isAttacking) || !isGiant){
            rb.velocity = Vector3.zero;            
            animator.Play("Hurt");
        } 
        isHurt = true;
    }


    public void debugTest(){
        Debug.Log("Test");
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag == "Ground"){
            isGrounded = true;
            state = State.Walking;
        }
    }

    private void OnCollisionExit(Collision other){
        if(other.transform.tag == "Ground"){
            isGrounded = false;
            state = State.Falling;
        }
    }

    private void fixRotation(){        
        var q = Quaternion.LookRotation(new Vector3(sr.transform.position.x, sr.transform.position.y, sr.transform.position.z + direction) - sr.transform.position);
        sr.transform.rotation = Quaternion.RotateTowards(sr.transform.rotation, q, flipSpeed * Time.deltaTime);        
    }

    public void StopHurt(){
        isHurt = false;
        animator.Play("Walking");                
    }
    public void StopAttack(){
        isAttacking = false;   
        Debug.Log("Melee enemy stopped attacking");
        animator.Play("Idle");
    }

    IEnumerator DelayedStopHurt(){
        yield return new WaitForSeconds(0.5f);
        StopHurt();
    }

    public void ArmorAttackSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.ARMORATTACK);
    }
    public void ArmorDeathSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.ARMORDEATH, 0.5f);
    }

    public void GiantAttackSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.GIANTATTACK);
    }
    public void GiantDeathSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.GIANTDEATH, 0.7f);
    }
    public void ElementalAttackSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.ELEMENTALATTACK);
    }
    public void ElementalDeathSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.ELEMENTALDEATH);
    }



    /* private void OnTriggerStay(Collider other){
        if(other.transform.CompareTag("Player")){
            Debug.Log("Player in range");
            state = State.Walking;
            trackTarget = other.gameObject;
        }
    } */
    /* private void OnTriggerExit(Collider other){
        if(other.transform.CompareTag("Player")){
            state = State.Idle;            
        }
    } */
}
