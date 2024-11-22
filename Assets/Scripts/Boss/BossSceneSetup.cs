using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneSetup : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartCutscene(){
        yield return new WaitForSeconds(1.5f);
    }
}
