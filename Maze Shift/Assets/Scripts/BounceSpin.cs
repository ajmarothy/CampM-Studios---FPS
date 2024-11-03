using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSpin : MonoBehaviour
{
    [SerializeField] float bounceSpeed;
    [SerializeField] float bounceAmount;
    [SerializeField] float rotationSpeed;

    private float heightStart;
    private float timeDeviation;

    Vector3 position;
    Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        heightStart = transform.localPosition.y;
        timeDeviation = Random.value * Mathf.PI * 2;
    }

    // Update is called once per frame
    void Update()
    {
        // bouncing animation
        float finalheight = heightStart + Mathf.Sin(Time.time * bounceSpeed + timeDeviation) * bounceAmount;
        position = transform.localPosition;
        position.y = finalheight;
        transform.localPosition = position;

        // spinning animation
        rotation = transform.localRotation.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }
}
