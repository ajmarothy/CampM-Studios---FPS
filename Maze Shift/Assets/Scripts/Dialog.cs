using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;

    private string[] lines;
    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
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
        }
    }
}