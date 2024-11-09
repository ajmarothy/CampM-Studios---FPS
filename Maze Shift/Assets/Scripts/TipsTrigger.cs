using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] tipLines;
    public GameObject tipPanel;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            if (tipPanel != null)
            {
                tipPanel.SetActive(true);
                TipText tipText = tipPanel.GetComponent<TipText>();
                if (tipText != null)
                {
                    tipText.ShowTip(tipLines);
                }
            }
        }
    }
}
