using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDoor : MonoBehaviour
{

    [SerializeField] GameObject moveableObject;
    [SerializeField] float moveHeight;
    [SerializeField] float moveDuration;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip doorClose;

    private bool playerInTrigger = false;
    private bool objectMoving = false;



    void Update()
    {
        if (Time.timeScale == 0)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
        else
        {
            if (!audioSource.isPlaying && objectMoving)
            {
                audioSource.UnPause();
            }
        }

        if (playerInTrigger && !objectMoving && Time.timeScale != 0)
        {
            audioSource.clip = doorClose;
            audioSource.Play();
            objectMoving = true;
            StartCoroutine(MoveHiddenObject(moveHeight, moveDuration));
        }

    }

    private IEnumerator MoveHiddenObject(float height, float duration)
    {
        if (moveableObject == null) yield break;

        Vector3 initialPosition = moveableObject.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, height, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(initialPosition.y, targetPosition.y, elapsedTime / duration);
            moveableObject.transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moveableObject.transform.position = targetPosition;
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
}
