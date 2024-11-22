using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class middlePointBehaviour : MonoBehaviour
{
    public Transform boss, player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(boss && player) transform.position = player.position + (boss.position-player.position)/2;
    }
}
