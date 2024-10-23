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


    public void respawn()
    {
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.Unpause();
    }

    public void NextScene()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        respawn();
    }


    public void OpenSettingsPause()
    {
        GameManager.instance.OpenSettings("pause");
    }

    public void OpenSettingsLose()
    {
        GameManager.instance.OpenSettings("lose");
    }

    public void CloseSettingsMenu()
    {
        GameManager.instance.CloseSettings();
    }

    public void SetVolume(float volume)
    {
        GameManager.instance.gameSettings.SetVolume(volume);
    }

    public void SetGraphicsQuality(int quality)
    {
        GameManager.instance.gameSettings.SetGraphicsQuality(quality);
    }

    public void SetDifficulty(int difficulty)
    {
        GameManager.instance.gameSettings.SetDifficulty(difficulty);
    }

    public void ApplySettings()
    {
        GameManager.instance.ApplySettings();
    }

    public void ResetSettings()
    {
        GameManager.instance.ResetSettings();
    }
}
