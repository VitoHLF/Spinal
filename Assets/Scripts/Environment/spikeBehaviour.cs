using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeBehaviour : MonoBehaviour
{
    public float globalGravity = -10, gravityScale = 1;
    private Vector3 gravity, velocity;
    private bool active = false;
    private float lifetime = 3f;
    
    void Start(){
        gravity = globalGravity * gravityScale * Vector3.up * 10;
    }
    private void FixedUpdate()
    {
        if(active){
            lifetime-=Time.deltaTime;
            velocity += gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }

        if(lifetime<=0f) Destroy(gameObject);

    }

    public void Activate(){
        active = true;
    }

    void OnCollisionEnter(Collision other){
        if(other.transform.tag == "Ground"){
            Destroy(gameObject);
        }
    }
}
