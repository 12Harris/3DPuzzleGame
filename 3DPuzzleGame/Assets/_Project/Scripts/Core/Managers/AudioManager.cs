// ============================================================================
// AUDIO MANAGER
// ============================================================================

using UnityEngine;
public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    protected override void Awake()
    {
        base.Awake();
        _musicSource = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;
    }

    public void PlayMusic(AudioClip clip, float volume = 0.5f)
    {
        _musicSource.clip = clip;
        _musicSource.volume = volume;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        _sfxSource.PlayOneShot(clip, volume);
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        _sfxSource.volume = Mathf.Clamp01(volume);
    }
}