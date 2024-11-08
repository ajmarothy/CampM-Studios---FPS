using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtMound : MonoBehaviour
{
    [SerializeField] float totalMoveHeight;
    [SerializeField] int steps;
    [SerializeField] float stepDelay;

    public PlayerController playerScript;
    [SerializeField] gunStats shovelGunStats;

    private bool isDigging = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerScript.currentGun == shovelGunStats && !isDigging)
        {
            StartCoroutine(DigDirtMound());
        }
    }

    private IEnumerator DigDirtMound()
    {
        isDigging = true;

        float stepHeight = totalMoveHeight / steps;

        for (int i = 0; i < steps; i++)
        {
            transform.position -= new Vector3(0, stepHeight, 0);
            yield return new WaitForSeconds(stepDelay);
        }

        isDigging = false;
    }
}
