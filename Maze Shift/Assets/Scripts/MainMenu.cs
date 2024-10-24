using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public void settings()
    {
        GameManager.instance.OpenSettings("Main");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void quitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
