using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballTrap : MonoBehaviour
{


    public Transform fire1;
    public Transform fire2;

    public GameObject fireball;

    [SerializeField] AudioSource cannonSource;
    [SerializeField] AudioClip cannonFire;
    [SerializeField] float cannonVol;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        StartCoroutine(shootFireball());
    }



    IEnumerator shootFireball()
    {
        cannonSource.PlayOneShot(cannonFire, cannonVol);
        GameObject projectile1 = Instantiate(fireball, fire1.position, fire1.rotation);                //I want it to be this but actually work Damage.damageType.fireBall, fire1);
        yield return new WaitForSeconds(2);
        cannonSource.PlayOneShot(cannonFire, cannonVol);
        GameObject projectile2 = Instantiate(fireball, fire2.position, fire2.rotation);
        yield return new WaitForSeconds(2);
    }


}
