using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpikeTrapAnim : MonoBehaviour
{

   // public static SpikeTrapAnim instance;

    public Animator spikeAnim;


    [SerializeField] AudioSource spikeSource;
    [SerializeField] AudioClip spikeOn;
    [SerializeField] AudioClip spikeOff;
    [SerializeField] float spikeVol;


    void Awake()
    {
        spikeAnim = GetComponent<Animator>();
       
        StartCoroutine(WorkingTrap());
    }


    IEnumerator WorkingTrap()
    {
        
        spikeAnim.SetTrigger("open");
        spikeSource.PlayOneShot(spikeOn, spikeVol);
        yield return new WaitForSeconds(2.5f);
        spikeAnim.SetTrigger("close");
        spikeSource.PlayOneShot(spikeOff, spikeVol);
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(WorkingTrap());

    }
}