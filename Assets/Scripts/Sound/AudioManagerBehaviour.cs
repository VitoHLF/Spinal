using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum MusicType{
        MAINMENU,
        STANDARD,
        BOSS
    }
    public enum EnemySoundType{        
        ARMORATTACK,
        BOWATTACK,
        ARMORDEATH,
        ARROWHIT,        
        GIANTATTACK,
        GIANTDEATH,        
        ELEMENTALATTACK,
        ELEMENTALDEATH,
        BOSSMISSILESPAWN,        
        BOSSMELEEATTACK,
        BOSSDEATH,
        EXPLOSION
    }
    public enum PlayerSoundType{
        ISHIT,
        MISSILEHIT,
        FIREBALLHIT,
        JUMP,
        DEATH
    }

    public enum EnvironmentSoundType{
        FALLINGSPIKEACTIVATE,
        FIREBALLJUMP,        
        WINDSTREAMENTER
    }

public class AudioManagerBehaviour : MonoBehaviour
{
    private static AudioManagerBehaviour instance;
    [Header("Clips")]
    public AudioClip[] musicClips;
    public AudioClip[] enemyClips;
    public AudioClip[] playerClips;
    public AudioClip[] environmentClips;
    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource enemySource;
    public AudioSource playerSource;
    public AudioSource environmentSource;
    [Header("Params")]  
    [Range(0,1)]public float globalVolume = 1f;

    void Awake(){        
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public static void PlayMusic(MusicType musicName, float volume = 1f, float pitch = 1f){
        instance.musicSource.pitch = pitch;
        instance.musicSource.volume = volume;
        instance.musicSource.clip = instance.musicClips[(int)musicName];
        instance.musicSource.Play();
        //instance.musicSource.Play(instance.musicClips[(int)musicName]);
    }
    public static void PlayEnemySound(EnemySoundType soundName, float volume = 1f, float pitch = 1f){
        instance.enemySource.pitch = pitch;
        instance.enemySource.PlayOneShot(instance.enemyClips[(int)soundName], volume);
    }
    public static void PlayPlayerSound(PlayerSoundType soundName, float volume = 1f, float pitch = 1f){
        instance.playerSource.pitch = pitch;
        instance.playerSource.PlayOneShot(instance.playerClips[(int)soundName], volume);

    }
    public static void PlayEnvironmentSound(EnvironmentSoundType soundName, float volume = 1f, float pitch = 1f){
        instance.environmentSource.pitch = pitch;
        instance.environmentSource.PlayOneShot(instance.environmentClips[(int)soundName], volume);
    }

    public static void ToggleSound(bool isActive){        
        float volume = (isActive) ? 1f:0f;
        instance.musicSource.volume = volume;
        instance.enemySource.volume = volume;
        instance.playerSource.volume = volume;
        instance.environmentSource.volume = volume;
    }

    public static void Kill(){
        Destroy(instance);
    }

}

