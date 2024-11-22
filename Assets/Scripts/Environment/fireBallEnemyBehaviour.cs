using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallEnemyBehaviour : MonoBehaviour
{
    public float jumpSpeed=10f, globalGravity=-10f, gravityScale=1f, flipSpeed = 1f;
    public GameObject parentFB;    
    public GameObject sprite;
    private Vector3 velocity;    

    void Start(){
        velocity = new Vector3(0f,0f,0f);
    }
    private void FixedUpdate(){
        velocity += globalGravity * gravityScale * Vector3.up * Time.deltaTime;

        if(velocity.y <= 0 && parentFB.transform.position.y >= transform.position.y){
            velocity.y = jumpSpeed;
            AudioManagerBehaviour.PlayEnvironmentSound(EnvironmentSoundType.FIREBALLJUMP, 0.3f);      
        }

        transform.position += velocity * Time.deltaTime;
        int direction = (int)Mathf.Sign(velocity.y);
        fixRotation(direction);
    }

    private void fixRotation(int direction){
        Vector3 directionVector = new Vector3(sprite.transform.position.x, sprite.transform.position.y - direction, sprite.transform.position.z) - sprite.transform.position;
        var q = Quaternion.LookRotation(directionVector, Vector3.up);
        sprite.transform.rotation = Quaternion.RotateTowards(sprite.transform.rotation, q, flipSpeed);
    }
}
