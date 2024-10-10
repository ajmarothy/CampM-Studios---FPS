using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;

    [SerializeField] TMP_Text enemyCounterText;
    public Image playerHPBar;
    public TMP_Text playerHPValue;

    public GameObject player;

    public bool isPaused;

    int enemyCounter;

    float timeScaleOG;

    bool GetPause()
    {
        return isPaused;
    }

    void SetPause(bool _isPaused)
    {
        isPaused = _isPaused;
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        timeScaleOG = Time.timeScale;

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                pause();
                menuActive = pauseMenu;
                menuActive.SetActive(GetPause());
            }
          
        }
    }

    public void pause()
    {
        SetPause(!GetPause());

        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

    }

    public void unpause()
    {
        SetPause(!GetPause());

        Time.timeScale = timeScaleOG;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemyCounter += amount;
        enemyCounterText.text = enemyCounter.ToString("F0");
        if(enemyCounter <= 0)
        {
            pause();
            menuActive = winMenu;
            menuActive.SetActive(true);
        }
    }

    public void youLose()
    {
        pause();
        menuActive = loseMenu;
        menuActive.SetActive(true);
    }
}
