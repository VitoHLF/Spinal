using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NPCBehaviour : MonoBehaviour
{
    public GameObject interactIndicator;
    private bool playerInRange = false;
    private bool isCurrentlyDialoguing= false;
    private GameObject player;
    public GameObject sceneChanger;
    
    [Header("Collectable settings")]
    public bool isHPCollectable = false;
    public bool isGreenCollectable = false;
    public bool isRedCollectable = false;
    public bool isFinalGate = false;
    public string collectableName;
    public string bossSceneName;
    void Start()
    {
        interactIndicator.SetActive(false);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && Input.GetButtonDown("Interact") && !isCurrentlyDialoguing){
            this.GetComponent<DialogueTrigger>().TriggerDialogue(gameObject);
            isCurrentlyDialoguing = true;
            if(isHPCollectable){
                player.GetComponent<playerBehaviour>().IncreaseHP();
                PlayerPrefs.SetInt(collectableName, 1);
            }
            if(isGreenCollectable){
                player.GetComponent<playerBehaviour>().hasDoubleJump = true;
                PlayerPrefs.SetInt(collectableName, 1);
                PlayerPrefs.SetInt("hasDoubleJump", 1);
            }
            if(isRedCollectable){
                player.GetComponent<playerBehaviour>().hasFireball = true;
                PlayerPrefs.SetInt(collectableName, 1);
                PlayerPrefs.SetInt("hasFireball", 1);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            interactIndicator.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            interactIndicator.SetActive(false);
            playerInRange = false;
        }
    }

    public void SetDialogueReady(){         
        StartCoroutine(MakeDialogueReady());
    }

    IEnumerator MakeDialogueReady(){
        yield return new WaitForSeconds(1f);
        if(isHPCollectable|| isGreenCollectable || isRedCollectable) {
            Destroy(gameObject);
        }
        if(isFinalGate){
            sceneChanger.GetComponent<SceneChanger>().CallScene(bossSceneName, 0);
        }
        else isCurrentlyDialoguing = false;
    }

}
