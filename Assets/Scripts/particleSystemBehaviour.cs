using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemBehaviour : MonoBehaviour
{
    public float lifeTime = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime<=0){
            Destroy(gameObject);
        }
    }
}
