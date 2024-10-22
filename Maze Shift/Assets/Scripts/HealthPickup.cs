using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] healthStats health;

    private void Start()
    {
        health.healItem = 0;
        health.healItemMax = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(health.healItem < health.healItemMax)
            {
                GameManager.instance.playerScript.GetHealth(health);
                health.healItem++;
                Destroy(gameObject);
            }
        }
    }
}
