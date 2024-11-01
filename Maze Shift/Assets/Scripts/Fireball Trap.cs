using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballTrap : MonoBehaviour
{


    public GameObject topCannon;
    public GameObject bottomCannon;
    public GameObject trapWall;

    public Transform fire1;
    public Transform fire2;


    // Start is called before the first frame update
    void Start()
    {
        topCannon = GameObject.FindWithTag("Top Cannon");
        bottomCannon = GameObject.FindWithTag("Bottom Cannon");
        trapWall = GameObject.FindWithTag("Fireball Wall");

        fire1 = topCannon.transform;
        fire2 = bottomCannon.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        StartCoroutine(shootFireball());
    }



    IEnumerator shootFireball()
    {
        GameObject projectile1 = Instantiate(topCannon);                //I want it to be this but actually work Damage.damageType.fireBall, fire1);
        yield return new WaitForSeconds(2);
        GameObject projectile2 = Instantiate(bottomCannon);
        yield return new WaitForSeconds(2);
    }


}
