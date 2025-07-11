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
        // Không giữ giữa scene

        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);

        musicAudioSound.volume = musicVolume;
        sfxAudioSound.volume = sfxVolume;

        if (musicClip != null)
        {
            musicAudioSound.clip = musicClip;
            musicAudioSound.loop = true;
            musicAudioSound.Play();
        }
    }
}
