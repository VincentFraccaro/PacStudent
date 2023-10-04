using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioClip initialClip;
    [SerializeField] private AudioClip backgroundMusic;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayInitialClip();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayBackgroundMusic();
        }
    }

    void PlayInitialClip()
    {
        audioSource.clip = initialClip;
        audioSource.loop = false; 
        audioSource.Play();
    }

    void PlayBackgroundMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; 
        audioSource.Play();

        this.enabled = false;
    }
}