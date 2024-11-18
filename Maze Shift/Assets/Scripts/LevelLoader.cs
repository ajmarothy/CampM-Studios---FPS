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
 

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
   
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

        operation.allowSceneActivation = true;
    }

}
