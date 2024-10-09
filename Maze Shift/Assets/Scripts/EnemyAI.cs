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
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {


        if (playerInRange)
        {
            playerDir = GameManager.instance.player.transform.position - transform.position;

            agent.SetDestination(GameManager.instance.player.transform.position);

            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }

            if (!IsShooting)
            {
                StartCoroutine(shoot());
            }
        }
    } 

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x,0,playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotateSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        IsShooting = true;

        Instantiate(bullet, shootPos.position,transform.rotation);

        yield return new WaitForSeconds(shootRate);

        IsShooting = false;
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamageColor());
        if( HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
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
