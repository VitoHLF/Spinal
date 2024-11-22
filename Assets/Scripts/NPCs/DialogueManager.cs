using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    //public bool isDialogueRunning;
    public GameObject dialogueBox;
    private GameObject player, currentNPC;
    private Queue<string> sentences;
    private bool skipEnabled = false;
    private bool isDialoguing = false;
    private float skipTimer = 0.1f;

    void Start()
    {
        sentences = new Queue<string>();
        player = GameObject.Find("Player");        
    }

    void Update()
    {
        skipTimer-=Time.deltaTime;
        skipEnabled = skipTimer <= 0f;
        
        if(Input.GetButtonDown("Interact") && sentences.Count>=0 && skipEnabled && isDialoguing){
            Debug.Log("Skiping dialogue");
            DisplayNextSentence();
        } 
    }

    public void StartDialogue(Dialogue dialogue, GameObject npc){
        isDialoguing = true;
        skipTimer = 0.1f;
        dialogueBox.SetActive(true);        
        /* Time.timeScale = 0; */
        currentNPC = npc;
        nameText.text = dialogue.name;
        player.GetComponent<playerBehaviour>().setControl(false);
        player.GetComponent<playerBehaviour>().Halt();

        sentences.Clear();
        foreach(string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence(){
        if(sentences.Count == 0){
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    void EndDialogue(){
        /* Time.timeScale = 1f; */
        StopAllCoroutines();
        sentences.Clear();
        player.GetComponent<playerBehaviour>().setControl(true);
        if(currentNPC) currentNPC.GetComponent<NPCBehaviour>().SetDialogueReady();
        dialogueBox.SetActive(false); 
        isDialoguing = false;       
        //Debug.Log("Dialogue ended");
    }

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
