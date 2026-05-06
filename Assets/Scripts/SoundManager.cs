using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1.0f; // optional: Lautstärke pro Sound

        public SoundEntry(string name, AudioClip clip, float volume = 1.0f)
        {
            this.name = name;
            this.clip = clip;
            this.volume = volume;
        }
    }

    public List<SoundEntry> sounds;   // sichtbar im Inspector

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();

    }

    public void Play(string name)
    {
        SoundEntry soundEntry = sounds.Find(s => s.name == name);
        if (soundEntry != null)
        {
            audioSource.PlayOneShot(soundEntry.clip, soundEntry.volume);
        }
        else
        {
            Debug.LogWarning("Sound nicht gefunden: " + name);
        }
    }

    public void Play(string name, float volume)
    {
        SoundEntry soundEntry = sounds.Find(s => s.name == name);
        if (soundEntry != null)
        {
            audioSource.PlayOneShot(soundEntry.clip, volume);
        }
        else
        {
            Debug.LogWarning("Sound nicht gefunden: " + name);
        }
    }

    public void Play(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning("AudioClip ist null");
        }
    }
}