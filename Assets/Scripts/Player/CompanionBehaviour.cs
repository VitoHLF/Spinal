using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionBehaviour : MonoBehaviour
{
    private Transform target;
    public SpriteRenderer sprite;
    public float pullForce = 1f; 
    [Range(0,1)]
    public float dampen = 0.9f;
    public float flipSpeed = 200f;
    private Vector3 velocity, force;
    private int lastDirection = 1;


    private void Start() {
        target = GameObject.Find("Player").transform;
        transform.position = target.position + Vector3.up * 5;
    }
    private void FixedUpdate() {        
        Vector3 posToTarget = target.position - transform.position;
        
        if (posToTarget.magnitude > 2){
            force = posToTarget * pullForce;
            velocity = velocity * dampen;
            velocity += force * Time.deltaTime;
            lastDirection = (int)Mathf.Sign(posToTarget.x);
            transform.position += velocity * Time.deltaTime;
        }else{
            //velocity = Vector3.zero;
        }

            fixRotation(lastDirection);
    }

    private void fixRotation(int direction){        
        var q = Quaternion.LookRotation(new Vector3(sprite.transform.position.x, sprite.transform.position.y, sprite.transform.position.z + direction) - sprite.transform.position);
        sprite.transform.rotation = Quaternion.RotateTowards(sprite.transform.rotation, q, flipSpeed * Time.deltaTime);        
    }
}
