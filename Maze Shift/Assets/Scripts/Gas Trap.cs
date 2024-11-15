using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTrap : MonoBehaviour
{
    [SerializeField] int damage;

    private float gasTime;

    [SerializeField] AudioClip[] gas;
    [SerializeField] float gasVol;
    [SerializeField] AudioSource gasSource;




    private void Awake()
    {
        //gasSource.PlayOneShot(gas[Random.Range(0, gas.Length)], gasVol);
    }


    private void OnTriggerStay(Collider other)
    {
        gasTime += Time.deltaTime;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null )
        {
            if (gasTime > 0.5)
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
