using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettings
{
    void SetVolume(float  volume);
    float GetVolume();

    void SetGraphicsQuality(int quality);
    int GetGraphicsQuality();

    void ApplySettings();
    void ResetToDefaults();

    void SetDifficulty(int difficuly);
    int GetDifficulty();
}
