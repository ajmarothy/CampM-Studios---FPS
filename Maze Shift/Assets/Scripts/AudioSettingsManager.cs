using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour, ISettings
{
    [SerializeField] AudioMixer MasterMixer;

    float masterVolume;
    float musicVolume;
    float sfxVolume;

    void Start()
    {
        LoadAudioSettings();
    }

    public void SetMasterVolume(float Volume)
    {
        masterVolume = Volume;
        MasterMixer.SetFloat("MasterVolume", Volume);
        PlayerPrefs.SetFloat("MasterVolume", Volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float Volume)
    {
        musicVolume = Volume;
        MasterMixer.SetFloat("MusicVolume", Volume);
        PlayerPrefs.SetFloat("MusicVolume", Volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float Volume)
    {
        sfxVolume = Volume;
        MasterMixer.SetFloat("SFXVolume", Volume);
        PlayerPrefs.SetFloat("SFXVolume", Volume);
        PlayerPrefs.Save();
    }

    public void ApplySettings() { LoadAudioSettings(); }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void LoadAudioSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.90f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.90f);

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public void ResetToDefaults()
    {
        SetMasterVolume(0.75f);
        SetMusicVolume(0.90f);
        SetSFXVolume(0.90f);
        SaveSettings();
    }
}
