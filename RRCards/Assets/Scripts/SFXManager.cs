using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioSource audioSource;
    public AudioClip clickSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClick()
    {
        if (clickSound != null && audioSource != null)
            audioSource.PlayOneShot(clickSound);
    }
}
