using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followParent : MonoBehaviour
{
    public Transform target;
    public float pullForce = 1f; 
    [Range(0,1)]
    public float dampen = 0.9f;
    private Vector3 velocity, force;
    public bool isDetached = false;

    private void FixedUpdate() {        

        if(isDetached) return;
        
        Vector3 posToTarget = target.position - transform.position;
        if (posToTarget.magnitude > 0.1){
            force = posToTarget * pullForce;
            velocity = velocity * dampen;
            velocity += force * Time.deltaTime;
        }else{
            //velocity = Vector3.zero;
        }
            transform.position += velocity * Time.deltaTime;
    }

}
