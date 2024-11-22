using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileBehaviour : MonoBehaviour
{
    public float speed = 5.0f;
    public float lifetime = 30f;
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }    
    private void FixedUpdate()
    {
        lifetime-=Time.deltaTime;
        if(lifetime<=0) Destroy(gameObject);
        Vector3 horizontalVelocity = new Vector3(speed * Time.deltaTime, 0, 0);
        transform.position += horizontalVelocity;
    }

    void OnTriggerEnter(Collider other){
        if(other.transform.tag == "Ground"){
            Destroy(gameObject);
        }
        if(other.transform.tag == "Player"){
            AudioManagerBehaviour.PlayEnemySound(EnemySoundType.ARROWHIT);
            other.GetComponent<playerBehaviour>().IsHurt(transform.position.x, 1, 2f);
            Destroy(gameObject);
        }        
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
