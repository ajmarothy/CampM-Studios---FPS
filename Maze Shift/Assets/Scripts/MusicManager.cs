using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource menuMusicSource;

    public void PlayMenuMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Pause();
        }
        if (!menuMusicSource.isPlaying)
        {
            menuMusicSource.Play();
        }
    }

    public void PlayBackgroundMusic()
    {
        if(menuMusicSource.isPlaying)
        {
            menuMusicSource.Pause();
        }
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }
}
