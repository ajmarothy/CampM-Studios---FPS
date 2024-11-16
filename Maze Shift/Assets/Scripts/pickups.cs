using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class pickups : MonoBehaviour
{
    [SerializeField] gunStats gun;

    private void Start()
    {
        gun.ammoCurr = gun.ammoPerMag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.getGunStats(gun);
            Destroy(gameObject);
        }
    }
}
