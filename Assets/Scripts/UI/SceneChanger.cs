using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    /* public string targetScene; */
    public Animator sceneChangeAnimator;
    public Transform []spawnPoints;
    public bool isMenu = false;
    
    void Start(){
        if(!isMenu){
            GameObject player = GameObject.Find("PlayerParent");
            int spawnIndex = PlayerPrefs.GetInt("nextSpawn");
            if(spawnPoints != null && spawnPoints.Length > spawnIndex){
                player.transform.position = spawnPoints[spawnIndex].position;
            }
        }
    }
    void Update(){
        /* if(Input.GetKeyDown(KeyCode.T)){
            StartCoroutine(LoadScene(targetScene));
        } */
    }

    public void CallScene(string sceneName, int spawnPoint = 0){
        StartCoroutine(LoadScene(sceneName, spawnPoint));
    }

    private IEnumerator LoadScene(string targetScene, int spawnPoint){
        yield return null;

        PlayerPrefs.SetInt("nextSpawn", spawnPoint);
        sceneChangeAnimator.SetTrigger("fadeEffect");
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(targetScene);
    }    
}
