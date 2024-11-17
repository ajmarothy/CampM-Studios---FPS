using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtMound : MonoBehaviour
{
    [SerializeField] float totalMoveHeight;
    [SerializeField] int steps;
    [SerializeField] float stepDelay;
    [SerializeField] AudioClip digSound;
    [SerializeField] AudioSource audioSource;

    public PlayerController playerScript;
    [SerializeField] gunStats shovelGunStats;

    private bool isDigging = false;
    bool inTrigger;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerScript.currentGun == shovelGunStats && !isDigging && inTrigger)
        {
            StartCoroutine(DigDirtMound());
        }
    }

    private IEnumerator DigDirtMound()
    {
        isDigging = true;
        audioSource.PlayOneShot(digSound);
        float stepHeight = totalMoveHeight / steps;

        for (int i = 0; i < steps; i++)
        {
            transform.position -= new Vector3(0, stepHeight, 0);
            yield return new WaitForSeconds(stepDelay);
        }

        isDigging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }
}
