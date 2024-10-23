using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallofIllusion : MonoBehaviour
{

    [SerializeField] GameObject wallIllusion;

    Mesh filterOG;
    Material rendererOG;



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
         wallIllusion.GetComponent<MeshFilter>().mesh = filterOG;
        wallIllusion.GetComponent<MeshRenderer>().material = rendererOG;
       
    }

    private void OnTriggerExit(Collider other)
    {
        //wallIllusion?.SetActive(false);

        wallIllusion.GetComponent<MeshFilter>().mesh = null;
        wallIllusion.GetComponent <MeshRenderer>().material = null;

    }
}
