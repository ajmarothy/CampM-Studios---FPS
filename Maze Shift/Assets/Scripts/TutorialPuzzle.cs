using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPuzzle : MonoBehaviour
{
    [SerializeField] GameObject moveableObject;
    [SerializeField] float moveHeight;
    [SerializeField] float moveDuration;
    [SerializeField] List<GameObject> flames;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip doorOpen;

    private bool objectMoving = false;

    void Update()
    {
        if (!objectMoving && AreAllFlamesOut())
        {
            objectMoving = true;
            audioSource.PlayOneShot(doorOpen);
            StartCoroutine(MoveDoor(moveHeight, moveDuration));
        }
    }

    private IEnumerator MoveDoor(float height, float duration)
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

    private bool AreAllFlamesOut()
    {
        foreach (GameObject flame in flames)
        {
            if (flame.activeSelf) return false;
        }
        return true;
    }
}
