using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("-------------- Audio Source --------------")]
    [SerializeField] internal AudioSource musicSource;
    [SerializeField] internal AudioSource soundSource;

    [Header("-------------- Audio Clip --------------")]
    public AudioClip mainMenu;
    public AudioClip gamePlay;
    public AudioClip win;
    public AudioClip lose;

    public void PlayMusic(AudioClip clip)
    {
        StopMusic();
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        StopMusic();
        soundSource.clip = clip;
        soundSource.Play();
    }

    public void StopMusic()
    {
        if (soundSource.isPlaying || musicSource.isPlaying)
        {
            musicSource.Stop();
            soundSource.Stop();
        }
    }
}
