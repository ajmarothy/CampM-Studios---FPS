using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickups : MonoBehaviour
{
    [SerializeField] gunStats gun;
    [SerializeField] healthStats health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.getGunStats(gun);
            Destroy(gameObject);
            GameManager.instance.playerScript.GetHealth(health);
            Destroy(gameObject);
        }
    }
}
