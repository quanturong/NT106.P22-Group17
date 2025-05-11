using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicAudioSound;
    public AudioSource sfxAudioSound;

    public AudioClip musicClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 

        musicAudioSound.clip = musicClip;
        musicAudioSound.loop = true;
        musicAudioSound.Play();
    }
}
