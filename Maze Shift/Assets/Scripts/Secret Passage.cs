using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPassage : MonoBehaviour
{
    [SerializeField] GameObject secretPassage;

    Mesh frameOG;
    Material skinOG;



    // Start is called before the first frame update
    void Start()
    {
        secretPassage = GameObject.FindWithTag("Secret");
        
        frameOG = secretPassage.GetComponent<MeshFilter>().mesh;
        skinOG = secretPassage.GetComponent<MeshRenderer>().material;

       
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("Player"))
        {
            secretPassage.GetComponent<MeshFilter>().mesh = null;
            secretPassage.GetComponent<MeshRenderer>().material = null;
            secretPassage.GetComponent<MeshCollider>().enabled = false;
        }
       

    }

    private void OnTriggerExit(Collider other)
    {
        
       if (other.CompareTag("Player"))
        {
            secretPassage.GetComponent<MeshFilter>().mesh = frameOG;
            secretPassage.GetComponent<MeshRenderer>().material = skinOG;
            secretPassage.GetComponent<MeshCollider>().enabled = true;
        }

    }
}
