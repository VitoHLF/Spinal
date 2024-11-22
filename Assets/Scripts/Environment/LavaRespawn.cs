using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRespawn : MonoBehaviour
{
    public Transform checkPoint;
    public GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }    
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            player.GetComponent<playerBehaviour>().IsHurt(transform.position.x, 1, 0f);
            player.transform.position = checkPoint.position;
        }
    }
}
