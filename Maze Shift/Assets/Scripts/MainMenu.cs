using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, ISettings
{
    public ISettings gameSettings;
    [SerializeField] GraphicsManager graphicsManager;
    [SerializeField] AudioSettingsManager audioManager;
    [SerializeField] MusicManager musicManager;
    [SerializeField] ControlsManager controlsManager;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject graphicsSettingsMenu;
    [SerializeField] GameObject audioSettingsMenu;
    [SerializeField] GameObject controlsSettingsMenu;
    [SerializeField] GameObject gameplaySettingsMenu;

    private Stack<string> menuHistory = new Stack<string>();
    private string currentMenu;
    private string parentMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    #region Settings

    public void OpenMenu(string menuName)
    {
        if (currentMenu == menuName) return;
        if (!string.IsNullOrEmpty(currentMenu))
        {
            menuHistory.Push(currentMenu);
            SetMenuActive(currentMenu, false);
        }
        currentMenu = menuName;
        SetMenuActive(menuName, true);
    }
    public void OpenSettings(string fromMenu)
    {
        parentMenu = fromMenu;
        OpenMenu("settings");
    }

    public void OpenSubMenu(string subMenuName)
    {
        SetMenuActive(currentMenu, false);
        currentMenu = subMenuName;
        SetMenuActive(subMenuName, true);
    }

    public void CloseSubmenu()
    {
        SetMenuActive(currentMenu, false);
        currentMenu = "settings";
        SetMenuActive(currentMenu, true);
    }

    public void CloseSettings()
    {
        SetMenuActive(currentMenu, false);
        currentMenu = parentMenu;
        parentMenu = null;
        SetMenuActive(currentMenu, true);
    }
    private void SetMenuActive(string menuName, bool isActive)
    {
        switch (menuName)
        {
            case "settings":
                settingsMenu.SetActive(isActive);
                break;
            case "graphics":
                graphicsSettingsMenu.SetActive(isActive);
                break;
            case "audio":
                audioSettingsMenu.SetActive(isActive);
                break;
            case "controls":
                controlsSettingsMenu.SetActive(isActive);
                break;
            case "gameplay":
                gameplaySettingsMenu.SetActive(isActive);
                break;
            default: break;
        }
    }

    public void OpenGraphicsSettings()
    {
        OpenMenu("graphics");
    }

    public void OpenAudioSettings()
    {
        OpenMenu("audio");
    }

    public void OpenControlsSettings()
    {
        OpenMenu("controls");
    }
    
    public void OpenGameplaySettings()
    {
        OpenMenu("gameplay");
    }

    public void OpenMenuMusic()
    {
        musicManager.PlayMenuMusic();
    }

    public void CloseMenuMusic()
    {
        musicManager.PlayBackgroundMusic();
    }

    public void ApplySettings()
    {
        gameSettings.ApplySettings();
    }

    public void SaveSettings()
    {
        gameSettings.SaveSettings();
    }

    public void ResetToDefaults()
    {
        gameSettings.ResetToDefaults();
    }

    #endregion


}
