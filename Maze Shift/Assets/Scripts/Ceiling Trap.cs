using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingTrap : MonoBehaviour
{

    //[SerializeField] Transform top;
    [SerializeField] Transform bottom;

    [SerializeField] float time;

    [SerializeField] int damage;

    GameObject ceiling;

    bool isCrushing;

    Vector3 up;
    Vector3 down;

    // Start is called before the first frame update
    void Start()
    {
        up = transform.position;
        
        down = bottom.position;

        ceiling = GameObject.FindWithTag("Ceiling");
        
        StartCoroutine(crushing());
    }

    // Update is called once per frame
    //void Update()
    //{
       
    //}

    IEnumerator crushing()
    {
        isCrushing = true;
        transform.position = Vector3.Lerp(up, down, time);
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
