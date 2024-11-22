using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningWallBehaviour : MonoBehaviour
{
    public GameObject[] objectsToHide;
    public void OpenRoom(){
        Debug.Log("Hiding objects");
        foreach(GameObject sceneObject in objectsToHide){
            sceneObject.SetActive(false);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
