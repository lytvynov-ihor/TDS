using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource sceneAudioSource;

    void Awake()
    {
        // Ensure only one instance of the AudioManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Find the AudioSource in the scene
        sceneAudioSource = GetComponent<AudioSource>();
        if (sceneAudioSource == null)
        {
            Debug.LogError("AudioManager requires an AudioSource component on the same GameObject.");
        }

        // Optionally make the AudioManager persist between scenes
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
