using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneUI : MonoBehaviour
{
    public GameObject sceneChanger;
    void Start()
    {
        
    }    
    void Update()
    {
        
    }

    public void ReturnToMainMenu(){
        Debug.Log("Returning to main menu");
        sceneChanger.GetComponent<SceneChanger>().CallScene("MainMenu");
    }
}
