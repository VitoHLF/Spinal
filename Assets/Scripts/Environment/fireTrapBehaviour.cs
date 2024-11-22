using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireTrapBehaviour : MonoBehaviour
{
    public float cycleTimer = 5f;
    public ParticleSystem particles;
    public bool particlesActive;    
    private float timer;
    
    void Start(){
        timer = 5f;
    }
    void FixedUpdate()
    {
        if(particlesActive){
            var emission = particles.emission;
            emission.enabled = true;    
            var collision = particles.collision;
            collision.enabled = true;
        }
        else{            
            var emission = particles.emission;
            emission.enabled = false;
            var collision = particles.collision;
            collision.enabled = false;
        }

        timer-=Time.deltaTime;

        if(timer<0f){
            timer = cycleTimer;
            SwitchParticleState();
        }

        
    }

    void SwitchParticleState(){
        particlesActive = !particlesActive;
    }
}
