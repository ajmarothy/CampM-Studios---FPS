using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{

    [SerializeField] Renderer model;
    [SerializeField] GameObject glow;

    [SerializeField] float saveIconWait;

    private FadeInOut FadeInOut;

    [SerializeField] AudioSource cpSource;
    [SerializeField] AudioClip[] fountain;
    [SerializeField] float cpVol;

    // Start is called before the first frame update
    void Start()
    {
        glow = GameObject.FindWithTag("Glow");
        FadeInOut = GameObject.Find("Save Icon").GetComponent<FadeInOut>();

        //cpSource.PlayOneShot(fountain[Random.Range(0, 2)], cpVol);
    }

   
    private void OnTriggerEnter(Collider other)
    {
        cpSource.Stop();

        if (glow == false)
            return;
       
        if (other.CompareTag("Player") && transform.position != GameManager.instance.getSpawnPos().transform.position)
        {
            cpSource.PlayOneShot(fountain[3], cpVol);
            GameManager.instance.getSpawnPos().transform.position = transform.position;
            glow.SetActive(false);
            SavingIcon();
        }
    }


    private void SavingIcon()
    {
        StartCoroutine(FadeOnSave());
    }

    IEnumerator FadeOnSave()
    {
        
        FadeInOut.FadeIn();
        yield return new WaitForSeconds(saveIconWait);
        FadeInOut.FadeOut();
    }
}
