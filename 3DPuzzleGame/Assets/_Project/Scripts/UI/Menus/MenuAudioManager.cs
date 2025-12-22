// MenuAudioManager.cs
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private float musicVolume = 0.5f;
    
    private AudioSource musicSource;
    
    private void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
    }
    
    private void Start()
    {
        PlayMenuMusic();
    }
    
    private void PlayMenuMusic()
    {
        if (menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.Play();
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
}