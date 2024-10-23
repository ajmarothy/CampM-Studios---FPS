using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpikeTrapAnim : MonoBehaviour
{

   // public static SpikeTrapAnim instance;

    public Animator spikeAnim; 

    
    void Awake()
    {
        spikeAnim = GetComponent<Animator>();
       
        StartCoroutine(WorkingTrap());
    }


    IEnumerator WorkingTrap()
    {
        
        spikeAnim.SetTrigger("open");
        yield return new WaitForSeconds(2.5f);
        spikeAnim.SetTrigger("close");
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(WorkingTrap());

    }
}