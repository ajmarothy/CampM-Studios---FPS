using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public float loadDuration = 5f;

    public void LoadLevel(string sceneName)
    {
        Debug.Log($"Attempting to load scene: {sceneName}");

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' does not exist or is not added to Build Settings.");
            return;
        }

        loadingScreen.SetActive(true);
        StartCoroutine(LoadWithFixedDuration(sceneName));
    }

    IEnumerator LoadWithFixedDuration(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < loadDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / loadDuration);
            slider.value = progress;

            yield return null;
        }

        Debug.Log($"Loading complete. Activating scene: {sceneName}");
        operation.allowSceneActivation = true;
    }

}
