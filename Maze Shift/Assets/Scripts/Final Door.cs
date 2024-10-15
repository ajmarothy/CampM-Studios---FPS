using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    [SerializeField] GameObject finalDoor;
    [SerializeField] float moveHeight;
    [SerializeField] float moveDuration;

    bool objectMoving = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetEnemyCounter() <= 0 && !objectMoving)
        {
            objectMoving = true;
            StartCoroutine(MoveHiddenObject(moveHeight, moveDuration));
        }
    }

    private IEnumerator MoveHiddenObject(float height, float duration)
    {
        if (finalDoor == null) yield break;

        Vector3 initialPosition = finalDoor.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, height, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(initialPosition.y, targetPosition.y, elapsedTime / duration);
            finalDoor.transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        finalDoor.transform.position = targetPosition;
    }
}

