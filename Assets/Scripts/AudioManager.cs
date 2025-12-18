using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    public AudioClip backgroundMusic;
    private AudioSource musicSource;

    [Header("Player Sounds")]
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    [Header("Game Sounds")]
    public AudioClip coinSound;

    [Header("Audio Sources")]
    private AudioSource sfxSource;
    private AudioSource footstepSource;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private bool isFootstepPlaying = false;
    private AudioClip currentFootstepClip;
    private bool isMuted = false;
    private float savedMusicVolume = 0.5f;
    private float savedSFXVolume = 1f;

    public System.Action<float> OnMusicVolumeChanged;
    public System.Action<float> OnSFXVolumeChanged;
    public System.Action<bool> OnMuteStateChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupAudioSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.loop = true;
        footstepSource.volume = sfxVolume * 0.6f;
        footstepSource.playOnAwake = false;
    }

    void Start()
    {
        PlayMusic();
    }

    #region Music Functions

    public void PlayMusic()
    {
        if (backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    #endregion

    #region Footstep Functions

    public void PlayWalk()
    {
        if (walkSound != null && (!isFootstepPlaying || currentFootstepClip != walkSound))
        {
            StopFootsteps();
            footstepSource.clip = walkSound;
            footstepSource.pitch = 1f;
            footstepSource.Play();
            isFootstepPlaying = true;
            currentFootstepClip = walkSound;
        }
    }

    public void PlayRun()
    {
        if (runSound != null && (!isFootstepPlaying || currentFootstepClip != runSound))
        {
            StopFootsteps();
            footstepSource.clip = runSound;
            footstepSource.pitch = 1.2f;
            footstepSource.Play();
            isFootstepPlaying = true;
            currentFootstepClip = runSound;
        }
    }

    public void StopFootsteps()
    {
        if (footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
        isFootstepPlaying = false;
        currentFootstepClip = null;
    }

    public bool IsFootstepSourceStopped()
    {
        return !isFootstepPlaying;
    }

    #endregion

    #region SFX Functions

    public void PlayJump()
    {
        PlaySFX(jumpSound);
    }

    public void PlayCoin()
    {
        PlaySFX(coinSound);
    }

    public void PlayDeath()
    {
        PlaySFX(deathSound);
        StopFootsteps();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && !isMuted)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    #endregion

    #region Volume Control & Mute

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            savedMusicVolume = musicVolume;
            savedSFXVolume = sfxVolume;
            SetAudioSourcesVolume(0f);
        }
        else
        {
            SetAudioSourcesVolume(savedMusicVolume);
        }
    }

    public void SetMasterVolume(float volume)
    {
        if (isMuted) return;

        musicVolume = Mathf.Clamp01(volume);
        sfxVolume = Mathf.Clamp01(volume);
        SetAudioSourcesVolume(musicVolume);
    }

    public void SetMusicVolume(float volume)
    {
        if (isMuted) return;

        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        if (isMuted) return;

        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        footstepSource.volume = sfxVolume * 0.6f;
    }

    private void SetAudioSourcesVolume(float volume)
    {
        musicSource.volume = volume;
        sfxSource.volume = volume;
        footstepSource.volume = volume * 0.6f;
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public float GetMusicVolume()
    {
        if (musicVolume == 0f)
        {
            musicVolume = 0.5f;
        }
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
    public void StopAllSounds()
    {
        musicSource.Stop();
        sfxSource.Stop();
        StopFootsteps();
        PlayMusic();
    }
    public void StopAllSoundsWithoutPlay()
    {
        musicSource.Stop();
        sfxSource.Stop();
        StopFootsteps();
        PlayMusic();
    }

    
    #endregion
}