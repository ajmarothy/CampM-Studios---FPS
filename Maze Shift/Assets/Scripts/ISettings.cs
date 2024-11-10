using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettings
{
    void ApplySettings();
    void SaveSettings();
    void ResetToDefaults();
}
