using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour, ISettings
{
    [SerializeField] AudioMixer MasterMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    float masterVolume;
    float musicVolume;
    float sfxVolume;

    private void Awake()
    {
        LoadAudioSettings();
    }

    public void SetMasterVolume(float sliderValue)
    {
        masterVolume = sliderValue;
        MasterMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicVolume = sliderValue;
        MasterMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxVolume = sliderValue;
        MasterMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void ApplySettings() 
    {
        LoadAudioSettings();
    }

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

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        MasterMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        MasterMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        MasterMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }

    public void ResetToDefaults()
    {
        SetMasterVolume(0.75f);
        SetMusicVolume(0.90f);
        SetSFXVolume(0.90f);
        SaveSettings();
        LoadAudioSettings();
    }
}
