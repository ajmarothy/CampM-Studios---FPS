using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitFall : MonoBehaviour
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
}
