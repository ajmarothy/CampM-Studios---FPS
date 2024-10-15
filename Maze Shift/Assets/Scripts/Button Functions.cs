using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public GameManager gameManager;
   
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


    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void OpenSettingsMenu()
    {
        GameManager.instance.OpenSettings();
    }


    public void CloseSettingsMenu()
    {
        GameManager.instance.CloseSettings();
    }

    public void SetVolume(float volume)
    {
        gameManager.gameSettings.SetVolume(volume);
    }

    public void SetGraphicsQuality(int quality)
    {
        gameManager.gameSettings.SetGraphicsQuality(quality);
    }

    public void ApplySettings()
    {
        gameManager.ApplySettings();
        gameManager.CloseSettings();
    }

    public void ResetSettings()
    {
        gameManager.ResetSettings();
        gameManager.CloseSettings();
    }
}
