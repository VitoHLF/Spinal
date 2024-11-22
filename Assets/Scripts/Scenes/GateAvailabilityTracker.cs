using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAvailabilityTracker : MonoBehaviour
{
    public GameObject gateAvailable, gateUnavailable;
    public bool isAvailable = false;
    public bool playerPrefOverride = false;
    void Start()
    {
        isAvailable = IntToBool(PlayerPrefs.GetInt("hasGreenCrystal")) && IntToBool(PlayerPrefs.GetInt("hasRedCrystal"));
        if(isAvailable || playerPrefOverride){
            gateAvailable.SetActive(true);
            gateUnavailable.SetActive(false);

        }
        else{
            gateAvailable.SetActive(false);
            gateUnavailable.SetActive(true);
        } 
    }
    
    void Update()
    {
        
    }

    private bool IntToBool(int value){
        if(value == 0) return false;
        return true;
    }    
}
