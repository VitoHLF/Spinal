using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneLoader : MonoBehaviour
{
    [Header("Default Pref Stats")]
    public int starterMaxHP = 2;
    
    [Header("Player Prefs")]
    public string nextSpawn = "nextSpawn";
    public string maxHP = "maxHP";
    public string tutorialHPTaken = "tutorialHPTaken";    
    public string leftRoomHPTaken = "leftRoomHPTaken";
    public string rightRoomHPTaken = "rightRoomHPTaken";
    public string upRoomHPTaken = "upRoomHPTaken";
    public string hasDoubleJump = "hasDoubleJump";
    public string hasFireball = "hasFireball";
    public string hasRedCrystal = "hasRedCrystal";
    public string hasGreenCrystal = "hasGreenCrystal";


    void Start(){
        
    }

    public void ResetPrefs(){
        PlayerPrefs.SetInt(nextSpawn, 0);
        PlayerPrefs.SetInt(maxHP, starterMaxHP);
        PlayerPrefs.SetInt(tutorialHPTaken, 0);
        PlayerPrefs.SetInt(leftRoomHPTaken, 0);
        PlayerPrefs.SetInt(rightRoomHPTaken, 0);
        PlayerPrefs.SetInt(upRoomHPTaken, 0);
        PlayerPrefs.SetInt(hasDoubleJump, 0);
        PlayerPrefs.SetInt(hasFireball, 0);
        PlayerPrefs.SetInt(hasRedCrystal, 0);
        PlayerPrefs.SetInt(hasGreenCrystal, 0);
    }
}

