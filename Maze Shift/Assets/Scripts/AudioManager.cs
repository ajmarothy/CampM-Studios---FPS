using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, ISettings
{
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioMixer gameAudioMixer;

    float masterVolume;
    float musicVolume;
    float sfxVolume;
    float backgroundVolume;

    void Start()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SetMasterVolume(float volume)
    {
        gameAudioMixer.SetFloat("Master", volume);
    }

    public void SetMusicVolume(float volume)
    {
        gameAudioMixer.SetFloat("Music", volume);
    }

    public void SetSFXVolume(float volume)
    {
        gameAudioMixer.SetFloat("SFX", volume);
    }

    public void SetBackgroundVolume(float volume)
    {
        gameAudioMixer.SetFloat("Background", volume);
    }

    public void ApplySettings() { LoadAudioSettings(); }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Master", masterVolume);
        PlayerPrefs.SetFloat("Music", musicVolume);
        PlayerPrefs.SetFloat("SFX", sfxVolume);
        PlayerPrefs.SetFloat("Background", backgroundVolume);
        PlayerPrefs.Save();
    }

    public void LoadAudioSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("Master", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("Music", 0.90f);
        sfxVolume = PlayerPrefs.GetFloat("SFX", 0.90f);
        backgroundVolume = PlayerPrefs.GetFloat("Background", 0.90f);

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        SetBackgroundVolume(backgroundVolume);
    }

    public void ResetToDefaults()
    {
        SetMasterVolume(0.75f);
        SetMusicVolume(0.90f);
        SetSFXVolume(0.90f);
        SetBackgroundVolume(0.90f);
    }
}
