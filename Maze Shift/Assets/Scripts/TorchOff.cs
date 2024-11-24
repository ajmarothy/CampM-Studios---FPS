using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchOff : MonoBehaviour
{
    [SerializeField] GameObject flame;
    [SerializeField] AudioClip flameToggle;
    [SerializeField] AudioSource audioSource;

    private bool playerInTrigger = false;
    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            toggleFlame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    void toggleFlame()
    {
        audioSource.PlayOneShot(flameToggle);

        if (flame.activeSelf)
        {
            flame.SetActive(false);
        }
        else
        {
            flame.SetActive(true);
        }
    }
}
