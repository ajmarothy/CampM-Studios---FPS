using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip acceptButtion;
    [SerializeField] private AudioClip resetButton;
    [SerializeField] private AudioClip cancelButtion;
    [SerializeField] private AudioClip openMenu;
    [SerializeField] private AudioClip openSubmenu;

    

   

    public void Resume()
    {
        PlaySoundEffect(acceptButtion);
        GameManager.instance.Unpause();
    }

    public void Restart()
    {
        PlaySoundEffect(resetButton);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.Unpause();
    }

    public void Quit()
    {
        PlaySoundEffect(cancelButtion);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Respawn()
    {
        PlaySoundEffect(resetButton);
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.Unpause();
       
    }

    public void NextScene()
    {
        PlaySoundEffect(acceptButtion);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Respawn();
    }

    public void OpenSettingsPause()
    {
        PlaySoundEffect(openMenu);
        GameManager.instance.OpenMenu("settings");
    }

    public void OpenSettingsLose()
    {
        PlaySoundEffect(openMenu);
        GameManager.instance.OpenMenu("settings");
    }

    public void OpenGraphicsSettings()
    {
        PlaySoundEffect(openSubmenu);
        GameManager.instance.OpenMenu("graphics");
    }

    public void OpenAudioSettings()
    {
        PlaySoundEffect(openSubmenu);
        GameManager.instance.OpenMenu("audio");
    }

    public void OpenControlSettings()
    {
        PlaySoundEffect(openSubmenu);
        GameManager.instance.OpenMenu("controls");
    }

    public void OpenGameplaySettings()
    {
        PlaySoundEffect(openSubmenu);
        GameManager.instance.OpenMenu("gameplay");
    }

    public void CloseSettingsMenu()
    {
        PlaySoundEffect(cancelButtion);
        GameManager.instance.CloseSettings();
    }

    public void CloseSubmenu()
    {
        PlaySoundEffect(cancelButtion);
        GameManager.instance.CloseSubmenu();
    }

    public void ApplySettings()
    {
        PlaySoundEffect(acceptButtion);
        GameManager.instance.ApplySettings();
    }

    public void ResetSettings()
    {
        PlaySoundEffect(resetButton);
        GameManager.instance.ResetToDefaults();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if(uiAudioSource != null && clip != null)
        {
            uiAudioSource.PlayOneShot(clip);
        }
    }


  
}
