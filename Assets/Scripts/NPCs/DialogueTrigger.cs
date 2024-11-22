using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private GameObject player;

    void Start(){
        player = GameObject.Find("Player");
    }

    public void TriggerDialogue(GameObject npc){
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, npc);
    }
}
