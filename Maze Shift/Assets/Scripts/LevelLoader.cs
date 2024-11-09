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

    public void loadLevel(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(loadWithFixedDuration(sceneIndex));
    }

    IEnumerator loadWithFixedDuration(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
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
