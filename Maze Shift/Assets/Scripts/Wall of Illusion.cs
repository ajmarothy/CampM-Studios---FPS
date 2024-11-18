using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallofIllusion : MonoBehaviour
{

    [SerializeField] GameObject wallIllusion;

    Mesh filterOG;
    Material rendererOG;

    [SerializeField] AudioSource magicSource;
    [SerializeField] AudioClip[] wallMagic;
    [SerializeField] float magicVol;

    // Start is called before the first frame update
    void Start()
    {
        wallIllusion = GameObject.FindWithTag("Illusion");
       // wallIllusion.SetActive(false);
       filterOG = wallIllusion.GetComponent<MeshFilter>().mesh;
        rendererOG = wallIllusion.GetComponent<MeshRenderer>().material;

        wallIllusion.GetComponent<MeshFilter>().mesh = null;
        wallIllusion.GetComponent<MeshRenderer>().material = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            magicSource.PlayOneShot(wallMagic[Random.Range(0, wallMagic.Length)], magicVol);
            wallIllusion.GetComponent<MeshFilter>().mesh = filterOG;
            wallIllusion.GetComponent<MeshRenderer>().material = rendererOG;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        //wallIllusion?.SetActive(false);
        if (other != null)
        {
            wallIllusion.GetComponent<MeshFilter>().mesh = null;
            wallIllusion.GetComponent<MeshRenderer>().material = null;
        }

    }
}
