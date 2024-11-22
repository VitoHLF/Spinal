using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneZoneBehaviour : MonoBehaviour
{
    public string sceneName;
    public int nextSpawn;
    public Transform thisParent;

    public void Start(){
        thisParent = transform.parent;
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            thisParent.GetComponent<SceneChanger>().CallScene(sceneName, nextSpawn);
        }
    }
}
