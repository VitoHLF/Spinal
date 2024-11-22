using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
//using UnityEditor.U2D;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerBehaviour : MonoBehaviour
{
    
    [Header("Stats")]        
    public int maxHP;
    public int currentHP;
    public float maxEnergy = 100;
    public float currentEnergy;
    public bool hasDoubleJump;
    public bool hasFireball;
    public float defaultHurtTime = 0.3f;
    public float magicMissileCooldown = 0.5f;
    private float magicMissileCost = 6f;
    public float fireballCooldown = 1f;
    private float fireballCost = 20f;
    private float energyCoolingRate = 10f;
    private float energyOverheatRate = 20f; 

    [Header("Physics Attributes")]
    public float speed = 5.0f;
    public float jThrust = 10.0f;
    public float gravityScale = 1.0f;
    public float  globalGravity = -10.0f; 
    public float flipSpeed = 1f;    
    public float knockBackDistance = 2f;
    public float bufferDefaultTime = 0.1f;
    public float fallSpeedLimit = 50f;
    public float windForce = 1f;
    

    [Header("Prefabs")]

    //Logic Attributes
    public GameObject missilePrefab;
    public GameObject fireballPrefab;
    public GameObject dustParticles;

    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Transform sprite;
    public Rigidbody rb;
    
    [Header("Private attributes")]
    private int lastDirection = 1;
    private int extraJumps = 0;
    private float gravityNormal, hurtTime;
    private bool isGrounded = false, holdingJump = false, hurt = false;// , onWall = false;
    private Vector3 velocity, force;
    private ContactPoint lastContactPointWall, lastContactPointFloor;
    private float sinceLastMissile=0f;
    private float sinceLastFireball=0f;

    private Collider lastPassThrough;
    private bool isOverheated = false;
    private float lastGrounded = 0f;
    private float lastJumpPress;
    public bool hasControl = true;
    


    void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //gravityNormal = -globalGravity * gravityScale;
        maxHP = Mathf.Clamp(PlayerPrefs.GetInt("maxHP"),2,6);
        currentHP = maxHP;
        hasDoubleJump = IntToBool(PlayerPrefs.GetInt("hasDoubleJump"));
        hasFireball = IntToBool(PlayerPrefs.GetInt("hasFireball"));
        currentEnergy = 0;
        magicMissileCost = 30;
        fireballCost = 60;
        energyCoolingRate = 24;
        energyOverheatRate = 12;
        defaultHurtTime = 1f;
    }

    void Update(){
        if(hasControl) controlManager();   
        fixRotation(lastDirection);

        maxHP = Mathf.Clamp(maxHP, 2, 6);

        /* if(Input.GetButtonDown("Interact")){            
            if(currentHP<maxHP)currentHP += 1;
        } */
        if(!isOverheated){
            currentEnergy -= energyCoolingRate * Time.deltaTime;
            if(currentEnergy>=100f) isOverheated = true;
        }else{
            currentEnergy -= energyOverheatRate * Time.deltaTime;
            if(currentEnergy<=0f) isOverheated = false;
        }
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        sinceLastFireball += Time.deltaTime;
        sinceLastMissile += Time.deltaTime; 

        lastGrounded -= Time.deltaTime; 
        lastJumpPress -= Time.deltaTime;      
    }
    private void FixedUpdate()
    {        
        defaultPhysicsUpdate();
    }

    void OnCollisionEnter(Collision other){        
        
        if(other.transform.tag == "Ground" || other.transform.tag == "PassThroughPlatform"){
            if(other.contacts[0].normal.y >= 0.5f){ //Chao - normal para cima
                lastContactPointFloor = other.contacts[other.contacts.Length-1];
                extraJumps = 1;
                if(lastJumpPress>0f){
                    rb.velocity = new Vector3(rb.velocity.x, jThrust, 0f);                    
                    AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.JUMP);
                }
                //isGrounded = true;
                //velocity.y = 0f;
            }

            if(Mathf.Abs(other.contacts[0].normal.x) >= 0.5){ //Parede - normal para os lados
                lastContactPointWall = other.contacts[other.contacts.Length-1];
                //transform.position -= new Vector3(other.contacts[0].normal.x * 0.1f, 0f, 0f);
                //velocity.x = 0f;
            }            

            /* var dustPosition = other.contacts[0].point + new Vector3(-GetComponent<Collider>().bounds.size.x /2, .5f, 0);
            Instantiate(dustParticles, dustPosition, Quaternion.identity); */
        }

        if(other.transform.CompareTag("Enemy") || other.transform.CompareTag("RangedEnemy")){
            IsHurt(other.transform.position.x, 1, 3);
        }
    }

    void OnCollisionStay(Collision other){

        
        if(other.transform.tag == "Ground" || other.transform.tag == "PassThroughPlatform"){
            if(other.contacts[0].normal.y >= 0.5f){
                lastContactPointFloor = other.contacts[other.contacts.Length-1];
                isGrounded = true;  
                lastGrounded = bufferDefaultTime;                    
            }
            if(other.contacts[0].normal.x >= 0.5){
                lastContactPointWall = other.contacts[other.contacts.Length-1];
            }            
        }
    }

    void OnCollisionExit(Collision other){
        if(other.transform.tag == "Ground" || other.transform.tag == "PassThroughPlatform"){
            isGrounded = false;            
            if(lastContactPointWall.normal.x >= 0.5){
                var wallPush = new Vector3(0f, 1f, 1f);
                force = new Vector3(0f, force.y, force.z);
            }

        }
    }
    void OnTriggerEnter(Collider other){
        double yNormal =  transform.position.y - other.ClosestPoint(transform.position).y;
        
        if(other.transform.tag == "PassThroughPlatform" && yNormal < 0.5f){
            lastPassThrough = other.gameObject.GetComponent<Collider>();
            Physics.IgnoreCollision(this.GetComponent<Collider>(), other.gameObject.GetComponent<Collider>(), true);
        }

        if(other.CompareTag("DamageZone")){
            IsHurt(other.transform.position.x, 1, 3f);
        }

        if(other.CompareTag("Boss")){
            IsHurt(other.transform.position.x, 1, 3f);
        }
        if(other.CompareTag("Wind")){
            AudioManagerBehaviour.PlayEnvironmentSound(EnvironmentSoundType.WINDSTREAMENTER);      
        }
    }
    void OnTriggerExit(Collider other){
        if(other.transform.tag == "PassThroughPlatform"){
            Physics.IgnoreCollision(this.GetComponent<Collider>(), lastPassThrough, false);
        }
    }

    void OnTriggerStay(Collider other){
        if(other.transform.tag == "PassThroughPlatform"){
            if(Input.GetAxis("Vertical")< -0.1f){// && Input.GetButton("Jump")){
                Physics.IgnoreCollision(this.GetComponent<Collider>(), other.gameObject.GetComponent<Collider>(), true);
            }
        }
        if(other.CompareTag("Wind")){
            rb.AddForce(Vector3.up * windForce);
        }
    }
    void OnParticleCollision(GameObject particle){
        Debug.Log("Hitting particle");
        if(particle.CompareTag("fireParticle") && !hurt) IsHurt(particle.transform.position.x, 1, 2f);
    }

    void defaultPhysicsUpdate(){
        
        var gravity = globalGravity * gravityScale * Vector3.up;
        if(holdingJump) gravity = gravity * 0.5f;
        
        if(rb.velocity.y < 0) rb.AddForce(gravity * 2f);
        else rb.AddForce(gravity);

        if(rb.velocity.y < -fallSpeedLimit) rb.velocity = new Vector3(rb.velocity.x, -fallSpeedLimit, 0f);

        /* if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f){

        } */
        if(hurtTime <= 0.5f && hasControl){
            float velX = rb.velocity.x + Input.GetAxis("Horizontal") * speed * Time.deltaTime * 7f;
            velX = Mathf.Clamp(velX, -speed, speed);
            rb.velocity = new Vector3(velX, rb.velocity.y, 0f); // Movimento horizontal
        }

        if(Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f){
            float velX = rb.velocity.x * 0.9f;
            rb.velocity = new Vector3(velX, rb.velocity.y, 0f); 
        }        
    }

    void controlManager(){ 
        if(hurtTime>=0f){
            hurtTime -= Time.deltaTime;
        }else{
            hurt = false;
        }

        if(Input.GetButton("Jump"))holdingJump = true;
        else holdingJump = false;

        if(Input.GetButtonDown("Jump") && rb.velocity.y < 0f)lastJumpPress = bufferDefaultTime;                

        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f){ //Flip do sprite            
            lastDirection = (int)Mathf.Sign(Input.GetAxis("Horizontal"));
            //transform.localScale = new Vector3(lastDirection, transform.localScale.y, transform.localScale.z); 
            //if(lastDirection<0) spriteRenderer.flipX = true;
            //else spriteRenderer.flipX = false;            

            animator.SetBool("Walking", true);
        }else animator.SetBool("Walking", false);

        if(lastGrounded > 0f && Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") > -0.1f){
            rb.velocity = new Vector3(rb.velocity.x, jThrust, 0f);  
            lastJumpPress = 0f;          
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.JUMP);
        }
        if(!isGrounded && Input.GetButtonDown("Jump") && extraJumps > 0 && Input.GetAxis("Vertical") > -0.1f && hasDoubleJump && lastGrounded <= 0f){
            rb.velocity = new Vector3(rb.velocity.x, jThrust * 0.8f, 0f);
            extraJumps--;
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.JUMP);
        }

        if(Input.GetButtonDown("Fire1") && sinceLastMissile > magicMissileCooldown && !isOverheated){            
            GameObject newMissile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.MISSILEHIT);
            newMissile.GetComponent<missileBehaviour>().SetDirection(lastDirection);
            sinceLastMissile = 0f;
            currentEnergy += magicMissileCost;
        }
        if(Input.GetButtonDown("Fire2") && hasFireball && sinceLastFireball > fireballCooldown && !isOverheated){            
            GameObject newMissile = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.FIREBALLHIT, 0.4f, 1.2f);
            newMissile.GetComponent<playerFBBehaviour>().SetDirection(lastDirection);
            sinceLastFireball = 0f;
            currentEnergy += fireballCost;
        }
    }

    public void IsHurt(float xPosition, int damage, float knockBackModifier = 0){
        if(!hurt){
            hurt = true;
            hurtTime = defaultHurtTime;        
            rb.velocity = new Vector3(transform.position.x - xPosition, Mathf.Abs(transform.position.x - xPosition), 0f) * knockBackModifier;             
            currentHP -= damage;
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.ISHIT);
            Debug.Log("Player Hurt");            
        }
        if(currentHP <= 0){
            animator.SetBool("isDeath", true);
            GameObject.Find("SceneLoader").GetComponent<SceneChanger>().CallScene(SceneManager.GetActiveScene().name, PlayerPrefs.GetInt("nextSpawn"));
            
        }
    }

    

    private void fixRotation(int direction){        
        var q = Quaternion.LookRotation(new Vector3(sprite.transform.position.x, sprite.transform.position.y, sprite.transform.position.z + direction) - sprite.transform.position);
        sprite.transform.rotation = Quaternion.RotateTowards(sprite.transform.rotation, q, flipSpeed * Time.deltaTime);        
    }

    private bool IntToBool(int value){
        if(value == 0) return false;
        return true;
    }    

    public bool GetOverheatStatus(){
        return isOverheated;
    }

    public void setControl(bool newStatus){
        rb.velocity = Vector3.zero;
        hasControl = newStatus;
    }

    public void IncreaseHP(){
        maxHP += 1;
        currentHP += 1;
        PlayerPrefs.SetInt("maxHP", maxHP);
    }

    public void Halt(){
        rb.velocity = Vector3.zero;
    }
}
