using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeActivator : MonoBehaviour
{
    public GameObject spike;
    private bool isActive = false;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            
            if(spike){
                spike.GetComponent<spikeBehaviour>().Activate();
                if(!isActive)AudioManagerBehaviour.PlayEnvironmentSound(EnvironmentSoundType.FALLINGSPIKEACTIVATE);      
                isActive = true;
            }
        }
    }
}
