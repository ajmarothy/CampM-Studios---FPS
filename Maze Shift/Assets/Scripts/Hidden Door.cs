using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : MonoBehaviour
{
    [SerializeField] GameObject hiddenDoor;
    [SerializeField] GameObject boss;

    bool doorRaising = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null && !doorRaising)
        {
            doorRaising = true;
            StartCoroutine(MoveHiddenDoor(5f, 3f));
        }
    }

    private IEnumerator MoveHiddenDoor(float height, float duration)
    {
        if (hiddenDoor == null) yield break;

        Vector3 initialPosition = hiddenDoor.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, height, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(initialPosition.y, targetPosition.y, elapsedTime / duration);
            hiddenDoor.transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hiddenDoor.transform.position = targetPosition;
    }
}
