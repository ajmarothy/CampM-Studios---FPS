using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickups : MonoBehaviour
{
    [SerializeField] private int magMultiplier = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.GetGunInventory().Count > 0)
                {
                    Debug.Log("Player picked up ammo.");
                    foreach (gunStats gun in player.GetGunInventory())
                    {
                        int ammoToAdd = gun.ammoPerMag * magMultiplier;
                        player.AddAmmo(gun, ammoToAdd);
                        Debug.Log($"Added {ammoToAdd} ammo to {gun.name}. New Total ammo: {gun.totalAmmo}");
                    }
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Player has no guns in inventory. Ammo not picked up.");
                }
            }
        }
    }
}
