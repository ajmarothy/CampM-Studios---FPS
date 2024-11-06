using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    [SerializeField] private Shield shieldPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if(playerController != null)
            {
                if(shieldPrefab != null)
                {
                    playerController.PickupShield(shieldPrefab);

                    Destroy(gameObject);
                }
              
            }
        }
    }
}
