using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] GameObject winMenu; // Reference to the win menu UI

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.pause(); 
            winMenu.SetActive(true); 
        }
    }
}
