using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSensorCollider : MonoBehaviour
{   
    public GameObject parent;
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ground")){
            parent.GetComponent<RangedBehaviour>().Flip();
        }
    }
}
