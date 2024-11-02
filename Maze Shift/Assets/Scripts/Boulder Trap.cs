using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrap : MonoBehaviour
{

    public Transform hole;

    public GameObject boulder;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator rollBoulder()
    {
        yield return new WaitForSeconds(1);
        GameObject projectile = Instantiate(boulder, hole.position, hole.rotation);
        yield return new WaitForSeconds(3);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }


        StartCoroutine(rollBoulder());

    }
}
