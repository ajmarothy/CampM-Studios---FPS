using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ISettings
{
    private float volume;
    private int graphicsQuality;

    public void ResetToDefaults()
    {
        SetVolume(1.0f);
        SetGraphicsQuality(2);
        ApplySettings();
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("GraphicsQuality", graphicsQuality);
        PlayerPrefs.Save();
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
        AudioListener.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void SetGraphicsQuality(int qualityLevel)
    {
        this.graphicsQuality = qualityLevel;
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    public int GetGraphicsQuality()
    {
        return graphicsQuality;
    }


}
