using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] dialogLines;
    private bool triggered = false;
    public GameObject dialogPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(true);
                DialogText dialogText = dialogPanel.GetComponent<DialogText>();
                if (dialogText != null)
                {
                    dialogText.StartDialog(dialogLines);
                }
            }
        }
    }
}