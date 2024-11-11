using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingsManager : ISettings
{
    public int resolutionIndex;
    public int qualityLevel;
    public bool fullscreen;
    public bool vSync;

    Resolution[] resolutions = Screen.resolutions;

    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();
        LoadResolutions();
    }

    public void LoadResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int resWidth = Screen.currentResolution.width;
        int resHeight = Screen.currentResolution.height;

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == resWidth && resolutions[i].height == resHeight)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void ApplyResolutionSettings()
    {
        // save current settings in case player wants to revert
        previousResolution = Screen.currentResolution;
        previousFullscreen = Screen.fullScreen;

        int selectedResolutionIndex = resolutionDropdown.value;
        Resolution selectedResolution = resolutions[selectedResolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);
        Screen.fullScreen = fullscreenToggle.isOn;

        // show confirmation dialog
        confirmationDialog.SetActive(true);

        // start countdown to auto-revert if no confirmation is made
        if(confirmationCoroutine != null) 
            StopCoroutine(confirmationCoroutine);
        confirmationCoroutine = StartCoroutine(ConfirmationCountdown());
    }

    IEnumerator ConfirmationCountdown()
    {
        int countdown = 10;
        while (countdown > 0)
        {
            countdownText.text = "Reverting in " + countdown + " seconds...";
            yield return new WaitForSeconds(1);
            countdown--;
        }
        RevertSettings();
    }

    // called when YES is pressed for confirmation
    public void ConfirmSettings()
    {
        confirmationDialog.SetActive(false);
        SaveSettings();
        if(confirmationCoroutine != null) StopCoroutine(confirmationCoroutine);
    }

    // called when NO is pressed
    public void RevertSettings()
    {
        confirmationDialog.SetActive(false);
        Screen.SetResolution(previousResolution.width, previousResolution.height, previousFullscreen);
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("GraphicsQuality", graphicsQualityDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("VSync", vSyncToggle.isOn? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        int savedResolution = PlayerPrefs.GetInt("Resolution", 0);
        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", 2);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        bool savedVSync = PlayerPrefs.GetInt("VSync", 1) == 1;

        resolutionDropdown.value = savedResolution;
        fullscreenToggle.isOn = savedFullscreen;
        graphicsQualityDropdown.value = savedQuality;
        vSyncToggle.isOn = savedVSync;

        ApplyResolutionSettings();
    }
}
