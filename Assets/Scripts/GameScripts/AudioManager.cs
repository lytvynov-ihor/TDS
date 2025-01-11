using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource sceneAudioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        sceneAudioSource = GetComponent<AudioSource>();
        if (sceneAudioSource == null)
        {
            Debug.LogError("AudioManager requires an AudioSource component on the same GameObject.");
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayClip(AudioClip clip)
    {
        if (sceneAudioSource != null && clip != null)
        {
            sceneAudioSource.clip = clip;
            sceneAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Cannot play clip. Either AudioSource or AudioClip is missing.");
        }
    }
}
