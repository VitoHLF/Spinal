using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject sceneChanger;
    public string sceneName;
    public int spawnPoint;
    public bool soundOn = true;
    public GameObject soundOnIcon;
    public GameObject soundOffIcon;

    void Awake(){        
    }

    public void StartButton(){
        sceneChanger.GetComponent<SceneChanger>().CallScene(sceneName, spawnPoint);
    }

    public void QuitGame(){
        Debug.Log("Exiting");
        Application.Quit();
    }
    
    public void ToggleVolume(){
        soundOn = !soundOn;
        soundOnIcon.SetActive(!soundOn);
        soundOffIcon.SetActive(soundOn);
        AudioManagerBehaviour.ToggleSound(!soundOn);
    }
}


