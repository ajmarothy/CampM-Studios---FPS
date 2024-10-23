using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{

    [SerializeField] int damage;

    bool trapOn;

    GameObject spikes;

    // Start is called before the first frame update
    void Start()
    {
        spikes = GameObject.FindWithTag("Spikes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (spikes.transform.position.y > 0)
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
            }

        }      


    }

    private void OnTriggerExit(Collider other)
    {
        if (spikes.transform.position.y > 0)
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
            }

        }


    }
}
