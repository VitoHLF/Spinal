using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEvents : MonoBehaviour
{
    public void ExplosionSound(){
        AudioManagerBehaviour.PlayEnemySound(EnemySoundType.EXPLOSION);      
    }
}
