using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxBehaviour : MonoBehaviour
{
    public BoxCollider hurtBox;
    
    private void Start(){
        hurtBox = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {        
        if(other.transform.CompareTag("Player")){
            Debug.Log("Player Hit");
            other.GetComponent<playerBehaviour>().IsHurt(transform.position.x, 1, 2f);
        }
    }
}
