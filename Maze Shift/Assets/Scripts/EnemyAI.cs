using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour , IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int rotateSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool IsShooting;
    bool playerInRange;

    Vector3 playerDir;

    Color colorOriginal;


    // Start is called before the first frame update
    void Start()
    {
        colorOriginal = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
      HP-=amount;

        StartCoroutine(flashDamageColor());


        if(HP<= 0)
        {
            Destroy(gameObject);
        }

    }

    IEnumerator flashDamageColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOriginal;
    }


}
