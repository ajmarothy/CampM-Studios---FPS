using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickups : MonoBehaviour
{
    [SerializeField] gunStats gun;

    private void Start()
    {
        gun.ammoCurr = gun.ammoMax;
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
