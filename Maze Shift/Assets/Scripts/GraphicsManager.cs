using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager : MonoBehaviour, ISettings
{
    Resolution[] resolutions;
    int selectedResolution;
    int qualityLevel;
    bool isFullscreen;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        LoadGraphicsSettings();
    }

    public void SetResolution(int resolutionIndex)
    {
        selectedResolution = resolutionIndex;
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, isFullscreen);
    }

    public void SetFullscreen(bool fullscreen)
    {
        isFullscreen = fullscreen;
        Screen.fullScreen = fullscreen;
    }

    public void SetQualityLevel(int level)
    {
        qualityLevel = level;
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    public void ApplySettings()
    {
        Screen.SetResolution(resolutions[selectedResolution].width, resolutions[selectedResolution].height, isFullscreen);
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", selectedResolution);
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1  : 0);
        PlayerPrefs.Save();
    }

    public void LoadGraphicsSettings()
    {
        selectedResolution = PlayerPrefs.GetInt("Resolution", resolutions.Length - 1);
        isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        qualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
        ApplySettings();
    }

    public void ResetToDefaults()
    {
        SetResolution(resolutions.Length - 1);
        SetQualityLevel(2);
        SetFullscreen(true);
    }
}
