using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    public PlayerController playerScript;

    private string[] lines;
    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialog(string[] dialogLines)
    {
        lines = dialogLines;
        index = 0;
        textComponent.text = string.Empty;
        gameObject.SetActive(true);
        StartCoroutine(TypeLine());
        Time.timeScale = 0;
        playerScript.isDialogActive = true;
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

       
    }

    IEnumerator WaitAndHide(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {

            gameObject.SetActive(false);

            Time.timeScale = 1;
            playerScript.isDialogActive = false;
        }
    }

    IEnumerator HideDialogAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.SetActive(false);

        Time.timeScale = 1;
        playerScript.isDialogActive = false;
    }
}