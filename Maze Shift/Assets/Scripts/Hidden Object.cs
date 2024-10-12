using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    [SerializeField] GameObject hiddenObject;
    [SerializeField] GameObject trigger;
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
        if (trigger == null && !objectMoving)
        {
            objectMoving = true;
            StartCoroutine(MoveHiddenObject(moveHeight, moveDuration));
        }
    }

    private IEnumerator MoveHiddenObject(float height, float duration)
    {
        if (hiddenObject == null) yield break;

        Vector3 initialPosition = hiddenObject.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, height, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(initialPosition.y, targetPosition.y, elapsedTime / duration);
            hiddenObject.transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hiddenObject.transform.position = targetPosition;
    }
}
