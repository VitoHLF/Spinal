using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBehaviour : MonoBehaviour
{
    public bool paused;
    public Animator pauseAnimator;
    public GameObject otherUI;    
    public GameObject sceneChanger;

    void Start()
    {        
        paused = false;
        SetPauseMenu(false);       
        if(!sceneChanger)sceneChanger = GameObject.Find("SceneLoader");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")){
            SetPauseMenu(!paused);
        }
    }

    public void SetPauseMenu(bool isPaused){
        Time.timeScale = (paused) ? 0:1;
        if(isPaused){
            pauseAnimator.Play("PauseSlideUp");
            Time.timeScale = 0;
        }else{
            pauseAnimator.Play("PauseSlideDown");
            Time.timeScale = 1;
        }
        if(otherUI) otherUI.SetActive(paused);
        paused = isPaused;
    }

    public void ExitGame(){
        AudioManagerBehaviour.Kill();
        pauseAnimator.Play("PauseSlideDown");
        Time.timeScale = 1f;
        sceneChanger.GetComponent<SceneChanger>().CallScene("MainMenu");
    }
}
