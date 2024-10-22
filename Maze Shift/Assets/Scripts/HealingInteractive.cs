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
                    StartCoroutine(flashHeal());
                }
            }
        }
        GameManager.instance.healRequest.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.healRequest.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.healRequest.gameObject.SetActive(false);
        }
    }

    IEnumerator flashHeal()
    {
        GameManager.instance.healing.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.healing.gameObject.SetActive(false);
    }
}
