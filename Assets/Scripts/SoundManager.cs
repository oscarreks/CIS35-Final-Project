using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton that handles the sound
/// </summary>
/// 

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;
    public AudioSource audio_source;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        audio_source.clip = clip;
        audio_source.Play();
    }
}
