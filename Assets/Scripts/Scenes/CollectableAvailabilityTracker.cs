using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAvailabilityTracker : MonoBehaviour
{
    public string playerPrefsCollectableName;
    private bool isCollected;
    void Start()
    {
        isCollected = IntToBool(PlayerPrefs.GetInt(playerPrefsCollectableName));
        if(isCollected){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IntToBool(int value){
        if(value == 0) return false;
        else return true;
    }
}
