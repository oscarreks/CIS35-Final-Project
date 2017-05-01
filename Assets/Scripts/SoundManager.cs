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

    public void Play(AudioClip clip, float vol)
    {
        audio_source.clip = clip;
        audio_source.volume = vol;
        audio_source.Play();
    }

    public void Play(AudioClip clip)
    {
        Play(clip, 0.2f);
    }
}
