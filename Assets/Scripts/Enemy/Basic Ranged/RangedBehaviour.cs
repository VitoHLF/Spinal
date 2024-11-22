using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBehaviour : MonoBehaviour
{
    [Header("Physics Parameters")]
    public float gravityScale = 1.0f;
    public float globalGravity = -10.0f; 
    public float speed = 1;
    public float flipSpeed = 500f;
    private bool isGrounded = false;
    public int direction = 1; // Direcao para a qual o inimigo esta virado

    [Header("Stats")]
    public float attackRange = 3f;
    public int HP = 3;

    [Header("References")]
    public Rigidbody rb;
    public GameObject sensor;
    public SpriteRenderer sr;
    public State state = State.Idle;
    private State prevState;
    public enum State{Idle, Walking, Attacking, Dying, Falling};
    public float sensorXLeft = -0.2f, sensorXRight = 1.6f; // Posicoes dos sensores de chao
    private GameObject trackTarget; // Alvo de perseguicao (jogador)
    public Animator animator;
    public GameObject arrowPrefab;
    private GameObject player;
    private bool isAttacking = false, isHurt = false, isDead = false;


    public void Start(){
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }
    public void FixedUpdate(){
        StateMachineController();
        defaultPhysicsUpdate();
    }

    public void Update(){
        fixRotation();
        /* if(Input.GetButtonDown("Fire1")){
            Attack();
        } */

    }

    

    private void StateMachineController(){
        if(isHurt || isDead) return;
        else{
        switch(state){            
            case State.Dying:
                rb.velocity = Vector3.zero;
                isDead = true;
                animator.Play("Death");                
                break;
            case State.Idle:                
                animator.Play("Idle");
                if(CheckPlayer()) state = State.Attacking;
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
                animator.Play("Attack");
                break;
            case State.Falling:
                if(CheckPlayer()){
                    state = State.Attacking;
                    break;                                
                }
                Walk();
                break;
        }}
    }    

    public void Walk(){
        if(CheckGround()){
            Vector3 velocity = new Vector3(speed*direction, 0, 0);
            rb.velocity = velocity;
        }else Flip();
    }

    public void Attack(){
        GameObject newMissile = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        newMissile.GetComponent<EnemyMissileBehaviour>().SetDirection(direction);
    }

    public void Die(){
        Destroy(gameObject);
    }

    public void Flip(){
        direction *= -1;
        if(direction>0) sensor.transform.position = new Vector3(transform.position.x + sensorXRight, sensor.transform.position.y,sensor.transform.position.z);
        else sensor.transform.position = new Vector3(transform.position.x + sensorXLeft, sensor.transform.position.y,sensor.transform.position.z);
    }

    public bool CheckGround(){
        RaycastHit hit;
        if(Physics.Raycast(sensor.transform.position, Vector3.down, out hit, 1f)){ //Checagem se ha chao no caminho
            if(hit.transform.CompareTag("Ground")) return true;
        }                
        return false;
    }

    private bool CheckPlayer(){
        Vector3 lookDirection = new Vector3(direction, 0, 0);
        int layerMask = 1 << 3;
        RaycastHit hit;
        if (Physics.Raycast(sensor.transform.position, lookDirection, out hit, attackRange, layerMask)) // Procura player a frente do objeto
        {   
            return true;
        }
        return false;
    }

    private void defaultPhysicsUpdate(){
        if(!isGrounded){ 
            Vector3 gravity = globalGravity * gravityScale * Vector3.up; // Gravidade
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        /* if(state != State.Walking){
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        } */
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

    public void IsHurt(int damage = 1){
        rb.velocity = Vector3.zero;
        HP -= damage;
        if(HP <= 0){
            state = State.Dying;
            isHurt = false;
            isAttacking = false;
            return;
        }
        if(player.transform.position.x < transform.position.x) direction = -1;
        animator.Play("Hurt");
        isHurt = true;        
    }

    public void StopHurt(){
        isHurt = false;
    }
    public void StopAttack(){
        isAttacking = false;
        Debug.Log("Ranged enemy finished attacking");
        animator.Play("Idle");
    }

    private void fixRotation(){        
        var q = Quaternion.LookRotation(new Vector3(sr.transform.position.x, sr.transform.position.y, sr.transform.position.z + direction) - sr.transform.position);
        sr.transform.rotation = Quaternion.RotateTowards(sr.transform.rotation, q, flipSpeed * Time.deltaTime);        
    }

}
