using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour, ISettings
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;

    Resolution[] resolutions;
    Resolution resolution;
    int selectedResolution;
    int qualityLevel;
    bool isFullscreen;
    bool isVSyncEnabled;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        LoadGraphicsSettings();
        PopulateResolutionDropdown();
    }

    private void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width}x{resolutions[i].height}";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        selectedResolution = resolutionIndex;
        resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
    }

    public void SetQualityLevel(int level)
    {
        qualityLevel = level;
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    public void SetFullscreen(bool fullscreen)
    {
        isFullscreen = fullscreen;
        Screen.fullScreen = fullscreen;
    }

    public void SetVSync(bool enabled)
    {
        isVSyncEnabled = enabled;
        QualitySettings.vSyncCount = isVSyncEnabled ? 1 : 0;
    }

    public void ApplySettings()
    {
        SetResolution(selectedResolution);
        SetQualityLevel(qualityLevel);
        SetFullscreen(isFullscreen);
        SetVSync(isVSyncEnabled);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", selectedResolution);
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1  : 0);
        PlayerPrefs.SetInt("VSync", isVSyncEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadGraphicsSettings()
    {
        int localResolution = PlayerPrefs.GetInt("Resolution", resolutions.Length - 1);
        selectedResolution = localResolution;
        qualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
        isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        isVSyncEnabled = PlayerPrefs.GetInt("VSync", 1) == 1;

        resolutionDropdown.value = selectedResolution;
        qualityDropdown.value = qualityLevel;
        fullscreenToggle.isOn = isFullscreen;
        vsyncToggle.isOn = isVSyncEnabled;

        ApplySettings();
    }

    public void ResetToDefaults()
    {
        SetResolution(resolutions.Length - 1);
        SetQualityLevel(2);
        SetFullscreen(true);
        SetVSync(true);

        resolutionDropdown.value = resolutions.Length - 1;
        qualityDropdown.value = 2;
        fullscreenToggle.isOn = true;
        vsyncToggle.isOn = true;
        GameManager.instance.playerScript.updatePlayerUI();
        SaveSettings();
    }

    // auto detect graphics settings based on user's hardware
    public void AutoDetectQuality()
    {
        if(SystemInfo.processorCount > 8 && SystemInfo.systemMemorySize >= 16000 && SystemInfo.graphicsMemorySize >= 4000)
        {
            SetQualityLevel(QualitySettings.names.Length - 1);
        }
        else if(SystemInfo.processorCount > 4 && SystemInfo.systemMemorySize >= 8000 && SystemInfo.graphicsMemorySize >= 2000)
        {
            SetQualityLevel(QualitySettings.names.Length / 2);
        }
        else { SetQualityLevel(0); }

        qualityDropdown.value = qualityLevel;
        qualityDropdown.RefreshShownValue();
    }
}
