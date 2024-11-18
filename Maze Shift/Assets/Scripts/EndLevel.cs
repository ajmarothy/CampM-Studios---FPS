using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] GameObject winMenu; // Reference to the win menu UI

    
    
    private void OnTriggerEnter(Collider other)
    {
        var collectionReference = GetComponentInParent<ArtifactCollected>();
        if (other.CompareTag("Player") && collectionReference.GetCollectedStatus() == true)
        {
            GameManager.instance.Pause(); 
            winMenu.SetActive(true); 
        }
        else if (other.CompareTag("Player")) //&& collectionReference.GetCollectedStatus() == false)
        {
            gameObject.GetComponentInParent<DialogExitTrigger>().FailedExit();
        }
    }
}
