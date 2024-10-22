using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{

    [SerializeField] Renderer model;
    [SerializeField] GameObject glow;

    // Start is called before the first frame update
    void Start()
    {
        glow = GameObject.FindWithTag("Glow");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform.position != GameManager.instance.getSpawnPos().transform.position)
        {
            GameManager.instance.getSpawnPos().transform.position = transform.position;
            glow.SetActive(false);
        }
    }
}
