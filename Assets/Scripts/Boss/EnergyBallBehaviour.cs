using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallBehaviour : MonoBehaviour
{
    private GameObject player;
    private Vector3 target, direction;
    public float speed;
    void Start()
    {
        player = GameObject.Find("Player");
        target = player.transform.position;
        direction = Vector3.Normalize(target - transform.position);
    }
    
    void FixedUpdate()
    {        
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            player.GetComponent<playerBehaviour>().IsHurt(transform.position.x, 1, 2f);
            Destroy(gameObject);
        }
    }
}
