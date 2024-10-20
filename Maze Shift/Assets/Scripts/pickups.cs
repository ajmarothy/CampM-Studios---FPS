using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickups : MonoBehaviour
{
    [SerializeField] gunStats gun;
    //[SerializeField] healthStats health;

    // Start is called before the first frame update
    void Start()
    {
        //health.healthCurr = 0;
        
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
