using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        GameManager.instance.Unpause();
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.Unpause();
    }


    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public void Respawn()
    {
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.Unpause();
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Respawn();
    }


    public void OpenSettingsPause()
    {
        GameManager.instance.OpenMenu("settings");
    }

    public void OpenSettingsLose()
    {
        GameManager.instance.OpenMenu("settings");
    }

    public void OpenGraphicsSettings()
    {
        GameManager.instance.OpenMenu("graphics");
    }

    public void OpenAudioSettings()
    {
        GameManager.instance.OpenMenu("audio");
    }

    public void OpenControlSettings()
    {
        GameManager.instance.OpenMenu("controls");
    }

    public void OpenGameplaySettings()
    {
        GameManager.instance.OpenMenu("gameplay");
    }

    public void CloseSettingsMenu()
    {
        GameManager.instance.CloseSettings();
    }

    public void CloseSubmenu()
    {
        GameManager.instance.CloseSubmenu();
    }

    public void ApplySettings()
    {
        GameManager.instance.ApplySettings();
    }

    public void ResetSettings()
    {
        GameManager.instance.ResetToDefaults();
    }
}
