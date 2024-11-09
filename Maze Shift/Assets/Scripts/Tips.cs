using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private string[] lines;
    private int index;

    public PlayerController playerScript;

    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.activeSelf)
        {
            HideTip();
        }
    }

    public void ShowTip(string[] tipLines)
    {
        lines = tipLines;
        index = 0;
        DisplayCurrentLine();
        gameObject.SetActive(true);
        Time.timeScale = 0;
        playerScript.isDialogActive = true;
    }

    void DisplayCurrentLine()
    {
        textComponent.text = lines[index];
    }

    void HideTip()
    {
        StartCoroutine(HideTipCoroutine());
    }

    private IEnumerator HideTipCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.SetActive(false);
        Time.timeScale = 1;
        playerScript.isDialogActive = false;
    }
}