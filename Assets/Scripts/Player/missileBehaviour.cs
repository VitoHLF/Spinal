using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileBehaviour : MonoBehaviour
{
    public float speed = 5.0f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;    
    public float lifetime = 2f;
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
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.MISSILEHIT, 1f, 0.8f);        
            other.gameObject.GetComponent<enemyBehaviour>().IsHurt();            
            StartCoroutine(ExplodeAndKill());
        }
        if(other.transform.tag == "RangedEnemy"){
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.MISSILEHIT, 1f, 0.8f);        
            other.gameObject.GetComponent<RangedBehaviour>().IsHurt();            
            StartCoroutine(ExplodeAndKill());
        }
        if(other.transform.tag == "Boss"){
            AudioManagerBehaviour.PlayPlayerSound(PlayerSoundType.MISSILEHIT, 1f, 0.8f);        
            other.gameObject.GetComponent<BossHitBoxTracker>().IsHurt();            
            StartCoroutine(ExplodeAndKill());
        }
        if(other.CompareTag("Ground")){            
            StartCoroutine(ExplodeAndKill());
        }
    }



    private IEnumerator ExplodeAndKill(){
        
        //yield return null;        
        speed = 0f;
        animator.Play("MagicMissileExplosion");
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
