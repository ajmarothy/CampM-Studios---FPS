using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;


    public GameObject player;

    bool isPaused;

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
                menuActive.SetActive(isPaused);
            }
        }
    }

    public void pause()
    {
        isPaused = !isPaused;

        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

    }


    public void unpause()
    {
        isPaused = !isPaused;

        Time.timeScale = timeScaleOG;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        menuActive.SetActive(isPaused);
        menuActive = null;
    }
}
