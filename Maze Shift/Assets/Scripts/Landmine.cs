using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{

    [SerializeField] int damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage playerDamage = other.GetComponent<IDamage>();
            if (playerDamage != null)
            {
                playerDamage.takeDamage(damageAmount);
            }

            Destroy(gameObject);

        }
            
    }

}
