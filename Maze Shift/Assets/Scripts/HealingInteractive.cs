using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingInteractive : MonoBehaviour
{
    public healthStats healingSpring;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
            foreach (Collider collider in hitColliders)
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    healingSpring.Heal(player);
                    ShowHealingMessage();
                    StartCoroutine(HideHealingMessage());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.keyIndicatorText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.keyIndicatorText.gameObject.SetActive(false);
        }
    }

    private void ShowHealingMessage()
    {
        GameManager.instance.healingMessage.gameObject.SetActive(true);
    }

    IEnumerator HideHealingMessage()
    {
        yield return new WaitForSeconds(0.75f);
        GameManager.instance.healingMessage.gameObject.SetActive(false);
    }
}
