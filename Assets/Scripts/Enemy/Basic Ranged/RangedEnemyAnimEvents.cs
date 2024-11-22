using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAnimEvents : MonoBehaviour
{
    public GameObject parentObject;

    void Start(){
        
    }

    private void Attack(){
        parentObject.GetComponent<RangedBehaviour>().Attack();
    }

    public void StopAttack(){
        parentObject.GetComponent<RangedBehaviour>().StopAttack();
    }

    public void StopHurtAnim(){
        parentObject.GetComponent<RangedBehaviour>().StopHurt();
    }

    public void Death(){
        parentObject.GetComponent<RangedBehaviour>().Die();
    }

    public void AttackSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.BOWATTACK);
    }
    public void DeathSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.ARMORDEATH);
    }
}
