using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTrap : MonoBehaviour
{
    [SerializeField] int damage;

    

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null )
        {
            dmg.takeDamage(damage);
        }
    }



    private void OnTriggerExit(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damage);
        }
    }

}
