using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrap : MonoBehaviour
{

    [SerializeField] float totalTime;




    //Original Code

    //public Transform hole;
    //public GameObject boulder;

    //IEnumerator rollBoulder()
    //{
    //    GameObject projectile = Instantiate(boulder, hole.position, hole.rotation);       
    //    yield return new WaitForSeconds(1f);
    //}


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.isTrigger)
    //    {
    //        return;
    //    }
    //    StartCoroutine(rollBoulder());

    //}
}
