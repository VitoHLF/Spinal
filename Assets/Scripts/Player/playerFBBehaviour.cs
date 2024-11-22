using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFBBehaviour : MonoBehaviour
{
    public float speed = 3.0f;
    public int damage = 2;
    public SpriteRenderer spriteRenderer;
    public Animator animator;    
    private float lifetime = 2f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }    
    private void FixedUpdate()
    {
        lifetime-=Time.deltaTime;
        if(lifetime<=0) StartCoroutine(ExplodeAndKill());
        Vector3 horizontalVelocity = new Vector3(speed * Time.deltaTime, 0, 0);
        transform.position += horizontalVelocity;
    }

    void OnTriggerEnter(Collider other){        
        if(other.transform.tag == "Enemy"){
            other.gameObject.GetComponent<enemyBehaviour>().IsHurt(damage);
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.FIREBALLHIT, 0.4f, 1f);
            /* StartCoroutine(ExplodeAndKill()); */
        }
        if(other.transform.tag == "RangedEnemy"){
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.FIREBALLHIT, 0.4f, 1f);
            other.gameObject.GetComponent<RangedBehaviour>().IsHurt(damage);
            /* StartCoroutine(ExplodeAndKill()); */
        }
        if(other.transform.tag == "Boss"){
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.FIREBALLHIT, 0.4f, 1f);
            other.gameObject.GetComponent<BossHitBoxTracker>().IsHurt(damage);
            /* StartCoroutine(ExplodeAndKill()); */
        }
        if(other.transform.tag == "BurningWall"){
            Debug.Log("door hit");
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.FIREBALLHIT, 0.4f, 1f);
            other.transform.GetComponent<BurningWallBehaviour>().OpenRoom();
            StartCoroutine(ExplodeAndKill());
        }
        if(other.CompareTag("Ground")){
            StartCoroutine(ExplodeAndKill());
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.transform.tag == "BurningWall"){
            Debug.Log("door hit");
            other.transform.GetComponent<BurningWallBehaviour>().OpenRoom();
            StartCoroutine(ExplodeAndKill());
        }

        
    }

    private IEnumerator ExplodeAndKill(){
        yield return null;
        speed = 0f;
        animator.Play("FireBallExplosion");
        GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }
    

    public void SetDirection(float direction){
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = speed*direction;
        if(direction<0) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
        /* Vector3 newScale = new Vector3(direction*transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.localScale = newScale; */
    }
}
