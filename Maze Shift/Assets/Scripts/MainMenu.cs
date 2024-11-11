using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, ISettings
{
    public ISettings gameSettings;
    [SerializeField] GraphicsManager graphicsManager;
    [SerializeField] AudioSettingsManager audioManager;
    [SerializeField] ControlsManager controlsManager;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject graphicsSettingsMenu;
    [SerializeField] GameObject audioSettingsMenu;
    [SerializeField] GameObject controlsSettingsMenu;
    [SerializeField] GameObject gameplaySettingsMenu;
    private Stack<string> menuHistory = new Stack<string>();
    private string previousMenu;

    private void Awake()
    {
        graphicsManager.LoadGraphicsSettings();
        audioManager.LoadAudioSettings();
        controlsManager.LoadControlSettings();
        gameplayManager.LoadGameplaySettings();
    }

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
    public void OpenSettings(string fromMenu)
    {
        menuHistory.Push(fromMenu);
        settingsMenu.SetActive(true);
        graphicsSettingsMenu.SetActive(false);
        audioSettingsMenu.SetActive(false);
        controlsSettingsMenu.SetActive(false);
        gameplaySettingsMenu.SetActive(false);

        graphicsManager.LoadGraphicsSettings();
        audioManager.LoadAudioSettings();
        controlsManager.LoadControlSettings();
        gameplayManager.LoadGameplaySettings();
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        graphicsSettingsMenu.SetActive(false);
        audioSettingsMenu.SetActive(false);
        controlsSettingsMenu.SetActive(false);
        gameplaySettingsMenu.SetActive(false);

        if (menuHistory.Count > 0)
        {
            previousMenu = menuHistory.Pop();
            switch (previousMenu)
            {
                case "main":
                    mainMenu.SetActive(true);
                    break;
            }
        }
    }

    public void OpenGraphicsSettings()
    {
        settingsMenu.SetActive(false);
        graphicsSettingsMenu.SetActive(true);
    }
    public void OpenAudioSettings()
    {
        settingsMenu.SetActive(false);
        audioSettingsMenu.SetActive(true);
    }
    public void OpenControlsSettings()
    {
        settingsMenu.SetActive(false);
        controlsSettingsMenu.SetActive(true);
    }
    public void OpenGameplaySettings()
    {
        settingsMenu.SetActive(false);
        gameplaySettingsMenu.SetActive(true);
    }

    public void ApplySettings()
    {
        gameSettings.ApplySettings();
        SaveSettings();
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
