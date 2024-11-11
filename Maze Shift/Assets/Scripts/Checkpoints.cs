using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{

    [SerializeField] Renderer model;
    [SerializeField] GameObject glow;

    [SerializeField] float saveIconWait;

    private FadeInOut FadeInOut;

    // Start is called before the first frame update
    void Start()
    {
        glow = GameObject.FindWithTag("Glow");
        FadeInOut = GameObject.Find("Save Icon").GetComponent<FadeInOut>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform.position != GameManager.instance.getSpawnPos().transform.position)
        {
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
