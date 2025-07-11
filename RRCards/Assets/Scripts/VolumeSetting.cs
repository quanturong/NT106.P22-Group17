using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [Header("Slider")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio Sources")]
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    private void Start()
    {
        LoadVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        sfxAudioSource.volume = volume;
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("sfxVolume", 1f);

        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;

        SetMusicVolume();
        SetSFXVolume();
    }
}
