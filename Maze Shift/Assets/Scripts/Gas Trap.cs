using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTrap : MonoBehaviour
{
    [SerializeField] int damage;

    private float gasTime;

    

    private void OnTriggerStay(Collider other)
    {
        gasTime += Time.deltaTime;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null )
        {
            if (gasTime > 1)
            {
                dmg.takeDamage(damage);
                gasTime = 0;
            }
        }
    }



    //private void OnTriggerExit(Collider other)
    //{
    //    IDamage dmg = other.GetComponent<IDamage>();

    //    if (dmg != null)
    //    {
    //        dmg.takeDamage(damage);
    //    }
    //}

}
