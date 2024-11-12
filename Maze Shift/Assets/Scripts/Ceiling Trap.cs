using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingTrap : MonoBehaviour
{

    //[SerializeField] Transform top;
    [SerializeField] Transform bottom;

    [SerializeField] float totalTime;

    [SerializeField] int damage;

    [SerializeField] AudioSource smashSource;
    [SerializeField] AudioClip[] smash;
    [SerializeField] float smashVol;

    float time;

    //GameObject ceiling;

    bool isCrushing;

    Vector3 up;
    Vector3 down;

    // Start is called before the first frame update
    void Start()
    {
        time += Time.deltaTime;
        up = transform.position;
        
        down = bottom.position;

        //ceiling = GameObject.FindWithTag("Ceiling");
        
        StartCoroutine(crushing());
    }

    // Update is called once per frame
    //void Update()
    //{
    //    transform.position = Vector3.Lerp(up, down, time);
    //}

    IEnumerator crushing()
    {
        isCrushing = true;
        transform.position = Vector3.Lerp(up, down, totalTime/time);
        smashSource.PlayOneShot(smash[Random.Range(0,smash.Length)], smashVol);
        yield return new WaitForSeconds(0.5f);
        isCrushing = false;
        transform.position = up;
        yield return new WaitForSeconds(2.5f);

        StartCoroutine(crushing());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCrushing)
        {
            return;
        }

        else if (isCrushing)
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
            }
        }
    }

}
