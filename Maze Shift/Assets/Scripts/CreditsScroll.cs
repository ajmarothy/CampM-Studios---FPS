using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float stopYPosition = 3420.833f;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool hasStopped = false;

    void Update()
    {
        if (!hasStopped && transform.position.y < stopYPosition)
        {
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
        else if (!hasStopped)
        {
            hasStopped = true;
            StartCoroutine(WaitAndLoadMainMenu());
        }
    }

    private IEnumerator WaitAndLoadMainMenu()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
