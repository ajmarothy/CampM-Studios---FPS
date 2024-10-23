using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ISettings gameSettings;

    private GameObject menuActive;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] TMP_Text exitLevelText;
    [SerializeField] TMP_Text enemyCounterText;
    public TMP_Text ammoCur, ammoMax;
    public TMP_Text healItem, healMax;
    
    public PlayerController playerScript;
    public Image playerHPBar;
    public TMP_Text playerHPValue;
    public GameObject playerDamageScreen;
    public TMP_Text reloading;
    public TMP_Text keyIndicatorText;
    public TMP_Text healingMessage;

    public GameObject player;
    private bool isPaused;
    int enemyCounter;
    float timeScaleOG;
    string previousMenu;

    private GameObject spawnPos;



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

    // Start is called before the first frame update
    void Awake()
    {
        gameSettings = new GameSettings();
        LoadSettings();
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

    public void Pause()
    {
        SetPause(!GetPause());

        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

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
    }

    public void OpenSettings(string fromMenu)
    {
        //this method will open the settings menu
        settingsMenu.SetActive(true);
        previousMenu = fromMenu;
        if (fromMenu == "pause")
        {
            pauseMenu.SetActive(false);
        }
        else if (fromMenu == "lose")
        {
            loseMenu.SetActive(false);
        }
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        if(previousMenu == "pause")
        {
            pauseMenu.SetActive(true);
        }
        else if (previousMenu == "lose")
        {
            loseMenu.SetActive(true);
        }
    }

    public void ApplySettings()
    {
        gameSettings.ApplySettings();
        CloseSettings();
    }

    public void ResetSettings()
    {
        gameSettings.ResetToDefaults();
        CloseSettings();
    }

    public void ChooseDifficulty(int difficulty)
    {
        gameSettings.SetDifficulty(difficulty);
    }

    public void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", 2);
        int savedDifficulty = PlayerPrefs.GetInt("GameDifficulty", 2); // 1, 2 ,3 to set easy to hard; default normale
        gameSettings.SetVolume(savedVolume);
        gameSettings.SetGraphicsQuality(savedQuality);
    }
}
