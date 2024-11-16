using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour, ISettings
{
    [SerializeField] AudioMixer MasterMixer;
    [SerializeField] AudioSource sfxPreview;
    [SerializeField] AudioSource musicPreview;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider menuSlider;
    [SerializeField] TMP_Text masterLevelText;
    [SerializeField] TMP_Text musicLevelText;
    [SerializeField] TMP_Text sfxLevelText;
    [SerializeField] TMP_Text menuLevelText;

    float masterVolume;
    float musicVolume;
    float sfxVolume;
    float menuVolume;

    bool isInitializing = false;

    private void Awake()
    {
        isInitializing = true;
        LoadAudioSettings();
        UpdateSliderText();
        isInitializing = false;
    }

    public void OnSliderValueChanged(string type)
    {
        if (isInitializing) return;
        float volume;
        switch (type)
        {
            case "Master":
                volume = masterSlider.value;
                SetMasterVolume(volume);
                UpdateSliderText();
                break;
            case "Music":
                volume = musicSlider.value;
                SetMusicVolume(volume);
                UpdateSliderText();
                break;
            case "SFX":
                volume = sfxSlider.value;
                SetSFXVolume(volume);
                PlaySFXPreview();
                UpdateSliderText();
                break;
            case "Menu":
                volume = menuSlider.value;
                SetMenuVolume(volume);
                UpdateSliderText();
                break;
            default:
                break;
        }
        UpdateSliderText();
    }

    private void UpdateSliderText()
    {
        if (masterLevelText != null)
            masterLevelText.text = $"{Mathf.RoundToInt(masterSlider.value * 100)}%";
        if (musicLevelText!= null)
            musicLevelText.text = $"{Mathf.RoundToInt(musicSlider.value * 100)}%";
        if (sfxLevelText != null)
            sfxLevelText.text = $"{Mathf.RoundToInt(sfxSlider.value * 100)}%";
        if (menuLevelText != null)
            menuLevelText.text = $"{Mathf.RoundToInt(menuSlider.value * 100)}%";
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

    public void SetMenuVolume(float sliderValue)
    {
        menuVolume = sliderValue;
        MasterMixer.SetFloat("MenuVolume", Mathf.Log10(menuVolume) * 20);
        PlayerPrefs.SetFloat("MenuVolume", menuVolume);
        PlayerPrefs.Save();
    }

    public void PlaySFXPreview()
    {
        if (sfxPreview != null && sfxPreview.clip != null)
        {
            sfxPreview.volume = sfxSlider.value;
            sfxPreview.PlayOneShot(sfxPreview.clip);
        }
    }

    public void ApplySettings() 
    {
        LoadAudioSettings();
        UpdateSliderText();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("MenuVolume", menuVolume);
        PlayerPrefs.Save();
    }

    public void LoadAudioSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.90f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        menuVolume = PlayerPrefs.GetFloat("MenuVolume", 0.50f);

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        menuSlider.value = menuVolume;

        MasterMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        MasterMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        MasterMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        MasterMixer.SetFloat("MenuVolume", Mathf .Log10(menuVolume) * 20);
    }

    public void ResetToDefaults()
    {
        SetMasterVolume(0.50f);
        SetMusicVolume(0.50f);
        SetSFXVolume(0.50f);
        SetMenuVolume(0.50f);
        SaveSettings();
        LoadAudioSettings();
    }
}
