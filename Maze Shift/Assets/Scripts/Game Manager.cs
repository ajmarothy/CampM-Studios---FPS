using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour, ISettings
{
    public static GameManager instance;
    public ISettings gameSettings;
    public enum DifficultyLevel { Easy, Medium, Hard }
    public DifficultyLevel difficultyLevel = DifficultyLevel.Medium;

    [SerializeField] GraphicsManager graphicsManager;
    [SerializeField] AudioSettingsManager audioManager;
    [SerializeField] ControlsManager controlsManager;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] public GameObject loseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject graphicsSettingsMenu;
    [SerializeField] GameObject audioSettingsMenu;
    [SerializeField] GameObject controlsSettingsMenu;
    [SerializeField] GameObject gameplaySettingsMenu;
    [SerializeField] TMP_Text exitLevelText;
    [SerializeField] TMP_Text enemyCounterText;

    public GameObject player;
    public GameObject playerDamageScreen;
    public PlayerController playerScript;
    public Image playerHPBar;
    public Image batteryUI;

    public TMP_Text ammoCur, ammoTotal;
    public TMP_Text healItem, healMax;
    public TMP_Text currentBattery, maxBattery;
    public TMP_Text playerHPValue;
    public TMP_Text reloading;
    public TMP_Text keyIndicatorText;
    public TMP_Text healingMessage;

    public Image shieldHealthBar;
    public TMP_Text shieldHealthValue;
    public GameObject playerShieldUI;

    private Stack<string> menuHistory = new Stack<string>();
    public string currentMenu;

    private bool isPaused;
    int enemyCounter;
    float timeScaleOG;

    private GameObject spawnPos;
    private GameObject menuActive;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timeScaleOG = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        exitLevelText.enabled = false;
        spawnPos = GameObject.FindWithTag("Spawn Pos");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                Pause();
                menuActive = pauseMenu;
                menuActive.SetActive(GetPause());
            }
        }
    }

    #region Getters / Setters
    public GameObject getSpawnPos()
    {
        return spawnPos;
    }

    public void SetSpawnPos(GameObject _spawnPos)
    {
        spawnPos = _spawnPos;
    }

    public bool GetPause()
    {
        return isPaused;
    }

    public void SetPause(bool _isPaused)
    {
        isPaused = _isPaused;
    }

    public int GetEnemyCounter()
    {
        return enemyCounter;
    }
    #endregion

    public void Pause()
    {
        SetPause(!GetPause());
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        OpenMenuMusic();
    }

    public void Unpause()
    {
        SetPause(!GetPause());
        Time.timeScale = timeScaleOG;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
        CloseMenuMusic();
    }

    public void UpdateGameGoal(int amount)
    {
    
        enemyCounter += amount;
        enemyCounter = Mathf.Max(enemyCounter, 0);
        enemyCounterText.text = enemyCounter.ToString("F0");

        if (enemyCounter <= 0)
        {
            exitLevelText.enabled = true;
         
        }
    }

    public void YouLose()
    {
        Pause();
        menuActive = loseMenu;
        menuActive.SetActive(true);
        OpenMenuMusic();
    }

    public void ToggleShieldUI(bool toggle)
    {
        if(playerShieldUI != null)
        {
            playerShieldUI.SetActive(toggle);
        }
    }

    #region Settings
    public void OpenSettings(string fromMenu)
    {
        OpenMenu("settings");
        if(fromMenu == "pause")
        {
            pauseMenu.SetActive(false);
        }
        else if(fromMenu == "lose")
        {
            loseMenu.SetActive(false);
        }
    }

    public void CloseSettings()
    {
        CloseCurrentMenu();
    }

    public void OpenMenu(string menuName)
    {
        if (!string.IsNullOrEmpty(currentMenu))
        {
            menuHistory.Push(currentMenu);
            SetMenuActive(currentMenu, false);
        }
        currentMenu = menuName;
        SetMenuActive(menuName, true);
    }

    public void CloseCurrentMenu()
    {
        SetMenuActive(currentMenu, false);
        if(menuHistory.Count > 0)
        {
            currentMenu = menuHistory.Pop();
            SetMenuActive(currentMenu, true);
        }
        else
        {
            currentMenu = null;
            Unpause();
        }
    }

    private void SetMenuActive(string menuName, bool isActive)
    {
        switch (menuName)
        {
            case "pause":
                pauseMenu.SetActive(isActive);
                break;
            case "lose":
                loseMenu.SetActive(isActive);
                break;
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

    public void OpenMenuMusic()
    {
        Time.timeScale = 0;
        musicManager.PlayMenuMusic();
    }

    public void CloseMenuMusic()
    {
        Time.timeScale = timeScaleOG;
        musicManager.PlayBackgroundMusic();
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
