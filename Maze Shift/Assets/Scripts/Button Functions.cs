using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        GameManager.instance.Unpause();
        GameManager.instance.previousMenu = null;
        GameManager.instance.pauseMenu.SetActive(false);
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
        GameManager.instance.previousMenu = "pause";
        GameManager.instance.OpenSettings("pause");
    }

    public void OpenSettingsLose()
    {
        GameManager.instance.previousMenu = "lose";
        GameManager.instance.OpenSettings("lose");
    }

    public void CloseSettingsMenu()
    {
        GameManager.instance.CloseSettings();
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
