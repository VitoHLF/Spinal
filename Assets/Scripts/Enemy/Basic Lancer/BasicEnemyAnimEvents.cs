using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAnimEvents : MonoBehaviour
{
    public GameObject parentObject;

    public void StopAttack(){
        parentObject.GetComponent<enemyBehaviour>().StopAttack();
        Debug.Log("Melee Attack ended");
    }

    public void StopHurtAnim(){
        parentObject.GetComponent<enemyBehaviour>().StopHurt();
    }

    public void Death(){
        parentObject.GetComponent<enemyBehaviour>().Die();
    }

    public void ArmorAttackSound(){
        parentObject.GetComponent<enemyBehaviour>().ArmorAttackSound();
    }
    public void ArmorDeathSound(){
        parentObject.GetComponent<enemyBehaviour>().ArmorDeathSound();
    }

    public void GiantAttackSound(){
        parentObject.GetComponent<enemyBehaviour>().GiantAttackSound();
    }
    public void GiantDeathSound(){
        parentObject.GetComponent<enemyBehaviour>().GiantDeathSound();
    }
    public void ElementalAttackSound(){
        parentObject.GetComponent<enemyBehaviour>().ElementalAttackSound();
    }
    public void ElementalDeathSound(){
        parentObject.GetComponent<enemyBehaviour>().ElementalDeathSound();
    }


}
