using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [Header("Audio Clip")]
    public AudioClip musicBackground;
    public AudioClip buffEffect;
    public AudioClip damagedEffect;
    public AudioClip musicEndgame;
    public AudioClip meteorEffect;
    public AudioClip dashEffect;
    public AudioClip suddenDeathAlarm;
    public AudioClip swordSwingEffect;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // musicSource.clip = musicBackground;
        // musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void StartLoopMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void StopLoopMusic()
    {
        musicSource.loop = false;
        musicSource.Stop();
    }
}
