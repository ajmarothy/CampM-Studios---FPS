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
    [SerializeField] string masterVolumeParameter = "MasterVolume";
    [SerializeField] string menuVolumeParameter = "MenuVolume";
    [SerializeField] float musicAttenuation = -20f;
    [SerializeField] float attenuationDuration = 0.5f;

    float masterVolume;
    float musicVolume;
    float sfxVolume;
    float menuVolume;
    float originalMasterVolume;
    private float sfxCooldown = 0.05f;
    private float lastPlayedTime = 0f;

    bool isInitializing = false;
    bool isPlayingSFX = false;

    private void Awake()
    {
        MasterMixer.GetFloat(masterVolumeParameter, out originalMasterVolume);
        isInitializing = true;
        isPlayingSFX = false;
        LoadAudioSettings();
        UpdateSliderText();
        isInitializing = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log($"Application Focus: {focus}");
        if (focus)
        {
            MasterMixer.SetFloat(masterVolumeParameter, originalMasterVolume);
        }
        else
        {
            MasterMixer.SetFloat(masterVolumeParameter, -80f);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log($"Application Pause: ");
        if (!pause)
        {
            MasterMixer.SetFloat(masterVolumeParameter, originalMasterVolume);
        }
        else
        {
            MasterMixer.SetFloat(masterVolumeParameter, -80f);
        }
    }

    private void Update()
    {
        if (sfxPreview.isPlaying)
        { 
            sfxPreview.volume = sfxSlider.value; 
        }
        isPlayingSFX = false;
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
                if (sfxPreview.isPlaying) 
                {
                    sfxPreview.Stop();
                    isPlayingSFX = false;
                }
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
        if (sfxPreview != null && sfxPreview.clip != null && !sfxPreview.isPlaying)
        {
            isPlayingSFX = true;
            MasterMixer.SetFloat(menuVolumeParameter, musicAttenuation);
            sfxPreview.volume = sfxSlider.value;
            sfxPreview.PlayOneShot(sfxPreview.clip);
            StartCoroutine(ResetSFXPlayFlag(sfxPreview.clip.length));
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

    private IEnumerator ResetSFXPlayFlag(float duration)
    {
        yield return new WaitForSeconds(duration);
        isPlayingSFX = false;
    }
}
