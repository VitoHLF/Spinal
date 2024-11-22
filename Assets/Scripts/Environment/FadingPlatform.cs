using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    
    public float defaultTimerFade = 2f, defaultTimerAwake = 2f;
    private float timer;
    public BoxCollider boxCol;
    public SpriteRenderer sr;    
    public bool active = true, wakingUp = false;
    
    
    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        sr = GetComponent<SpriteRenderer>();        
    }

    void Update(){
        if(!active){
            timer -=  Time.deltaTime;
            if(timer<=0){
                boxCol.enabled = false;
                sr.enabled = false;
                wakingUp = true;                
                timer = defaultTimerAwake;
            }
        }
        if(wakingUp){
            timer -= Time.deltaTime;
            if(timer<=0){
                boxCol.enabled = true;
                sr.enabled = true;
                wakingUp = false;
                active = true;
            }
        }
    }

    void OnCollisionEnter(Collision other){
        if(other.transform.tag == "Player" && other.contacts[0].normal.y < 0.5){
            active = false;
            timer = defaultTimerFade;
        }
    }
}
