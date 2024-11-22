using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStartScript : MonoBehaviour
{
    public MusicType musicType;
    void Start()
    {
        AudioManagerBehaviour.PlayMusic(musicType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
