using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] EnemyStats enemyStats;

    [SerializeField] Image enemyHPBar;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator animator;

    private int HPCurrent;

    bool IsShooting;
    bool playerInRange;
    bool isRoaming;

    float angleToPlayer;
    float verticalVel;
    float originalStoppingDist;

    Vector3 playerDir;
    Vector3 lastPosition;
    Color colorOriginal;

    Coroutine someCo;

    public int GetOGhp()
    {
        return enemyStats.HPStart;
    }

    public int GetHP()
    {
        return HPCurrent;
    }

    // Start is called before the first frame update
    void Start()
    {
        HPCurrent = enemyStats.HPStart;
        updateEnemyUI();

        colorOriginal = model.material.color;
        GameManager.instance.UpdateGameGoal(1);
        lastPosition = transform.position;
        originalStoppingDist = agent.stoppingDistance;
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

        ApplyGravity();

        if (playerInRange && canSpotPlayer())
        {
            if (!isRoaming && agent.remainingDistance < 0.0f)
                someCo = StartCoroutine(roam());


        }
        else if (!playerInRange)
        {
            if(!isRoaming && agent.remainingDistance < 0.05f)
            {
                someCo = StartCoroutine(roam());
            }
        }
        


    }

    IEnumerator roam()
    {
        isRoaming = true;
        yield return new WaitForSeconds(enemyStats.roamPauseTime);

        agent.stoppingDistance = 0;
        Vector3 randomDistance = Random.insideUnitSphere * enemyStats.roamDistance;
        randomDistance += lastPosition;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDistance, out hit, enemyStats.roamDistance, 1);
        agent.SetDestination(hit.position);

        isRoaming = false;
        someCo = null;
    }

    void ApplyGravity()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, enemyStats.groundCheckDistance, enemyStats.groundLayer);

        if (isGrounded)
        {
            verticalVel = 0; // Reset vertical velocity when grounded
        }
        else
        {
            verticalVel += enemyStats.gravity * Time.deltaTime; // Apply gravity when not grounded
        }

        // Move the agent with the vertical velocity
        Vector3 movement = new Vector3(0, verticalVel, 0);
        agent.Move(movement * Time.deltaTime);
    }


    bool canSpotPlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= enemyStats.sightLineAngle)
            {

                float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (distanceToPlayer <= enemyStats.shootDistance)
                {
                    

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        faceTarget();
                    }

                    if (!IsShooting && angleToPlayer < enemyStats.shootAngle)
                    {
                        StartCoroutine(shoot());
                    }

                    agent.stoppingDistance = originalStoppingDist;

                    return true;
                }

                
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * enemyStats.rotateSpeed);
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
            agent.stoppingDistance = 0;
        }
    }

    IEnumerator shoot()
    {
        IsShooting = true;

        animator.SetTrigger("attackTrigger");
        Instantiate(enemyStats.bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(enemyStats.shootRate);

        IsShooting = false;
    }


    public void takeDamage(int amount)
    {
        HPCurrent -= amount;
        updateEnemyUI();
        
        StartCoroutine(flashDamageColor());

        if(someCo != null)
        {
            StopCoroutine(someCo);
            isRoaming = false;
        }
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (HPCurrent <= 0)
        {
            
            agent.speed = 0;
            animator.SetTrigger("deathTrigger");
            StartCoroutine(waitForDeathAnimation());
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
        enemyHPBar.fillAmount = (float)HPCurrent / enemyStats.HPStart;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, enemyStats.shootDistance);

        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            Gizmos.color = Color.green;
            Vector3 playerPosition = GameManager.instance.player.transform.position;
            Gizmos.DrawLine(transform.position, playerPosition);
        }
    }

   

    IEnumerator waitForDeathAnimation()
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animatorStateInfo.length;

        yield return new WaitForSeconds(animationLength);

        if (enemyStats.isLargeSpider)
        {
            for (int i = 0; i < enemyStats.numberOfMiniSpiders; i++)
            {

                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * enemyStats.spawnRadius;
                spawnPosition.y = transform.position.y; //keeping height of orignal spider

                Instantiate(enemyStats.miniSpiderPrefab, spawnPosition, Quaternion.identity);
            }
        }


        Destroy(gameObject);
        GameManager.instance.UpdateGameGoal(-1);
    }

    public int EnemyDifficulty(int difficulty)
    {
        difficulty = GameManager.instance.gameSettings.GetDifficulty();
        if(difficulty == 3)
        {
            HPCurrent *= 2;
            enemyStats.shootRate *= 2;
            enemyStats.shootDistance *= 2;
            // increase move speed like full time sprint
        }
        else if(difficulty == 1)
        {
            HPCurrent /= 2;
            enemyStats.shootRate /= 2;
            enemyStats.shootDistance /= 2;
            // move speed stays at normal speed
        }
        return difficulty;
    }
}
