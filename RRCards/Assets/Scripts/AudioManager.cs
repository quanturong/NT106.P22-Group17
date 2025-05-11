using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSound;
    public AudioSource sfxAudioSound;

    public AudioClip musicClip;

    void Start()
    {
        musicAudioSound.clip = musicClip;
        musicAudioSound.Play();
    }
}
