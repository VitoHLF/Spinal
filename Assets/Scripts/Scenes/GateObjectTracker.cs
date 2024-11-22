using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateObjectTracker : MonoBehaviour
{
    public string playerPrefObjName;
    public bool playerPrefOverride;
    public GameObject vfx;
    private bool isActive;
    void Start()
    {
        isActive = IntToBool(PlayerPrefs.GetInt(playerPrefObjName));
        if(isActive || playerPrefOverride){
            vfx.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IntToBool(int value){
        if(value == 0) return false;
        return true;
    }    
}
