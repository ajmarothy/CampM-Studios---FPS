using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour, ISettings
{
    public enum DifficultyLevel { Easy, Medium, Hard }
    public DifficultyLevel difficulty = DifficultyLevel.Medium;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        LoadGameplaySettings();
    }

    public void SetDifficulty(int level)
    {
        difficulty = (DifficultyLevel)level;
        gameManager.difficultyLevel = (GameManager.DifficultyLevel)difficulty;
    }

    public void ApplySettings()
    {
        gameManager.difficultyLevel = (GameManager.DifficultyLevel)difficulty;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Difficulty", (int)difficulty);
        PlayerPrefs.Save();
    }

    public void LoadGameplaySettings()
    {
        difficulty = (DifficultyLevel)PlayerPrefs.GetInt("Difficulty", (int)DifficultyLevel.Medium);
        ApplySettings();
    }

    public void ResetToDefaults()
    {
        SetDifficulty((int)DifficultyLevel.Medium);
    }
}
