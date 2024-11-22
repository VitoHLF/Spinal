using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitBoxTracker : MonoBehaviour
{
    public GameObject bossController;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void IsHurt(int damage = 1){
        bossController.GetComponent<BossControler>().IsHurt(damage);
    }

    public void Kill(){
        bossController.GetComponent<BossControler>().Kill();
    }
}
