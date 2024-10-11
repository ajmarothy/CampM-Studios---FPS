using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour , IDamage
{

    

    [SerializeField] Image enemyHPBar;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int rotateSpeed;
    [SerializeField] int sightLineAngle;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool IsShooting;
    bool playerInRange;
    int originalEnemyHP;

    float angleToPlayer;

    Vector3 playerDir;

    Color colorOriginal;



    public int GetOGhp()
    {
        return originalEnemyHP;
    }

    public int GetHP()
    {
        return HP;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalEnemyHP = HP;
        updateEnemyUI();
        
        colorOriginal = model.material.color;
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {


        if (playerInRange && canSpotPlayer())
        {
            

           
        }
    }


    bool canSpotPlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= sightLineAngle)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (!IsShooting)
                {
                    StartCoroutine(shoot());
                }

                return true;
            }
        }
        return false;
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
         updateEnemyUI();
        
        agent.SetDestination(GameManager.instance.player.transform.position);

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

    public void updateEnemyUI()
    {
        //GameManager.instance.enemyHPValue.text = (((float)HP / originalEnemyHP) * 100).ToString();
        enemyHPBar.fillAmount = (float)HP / originalEnemyHP;
    }
}
