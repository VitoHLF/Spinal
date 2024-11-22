using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCollider : MonoBehaviour
{   
    public GameObject parent;
    public bool testFlip = false;

    private void Update() {
        if(testFlip)        {
            testFlip = !testFlip;
            parent.GetComponent<enemyBehaviour>().Flip();
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ground")){
            parent.GetComponent<enemyBehaviour>().Flip();
        }
    }
}
