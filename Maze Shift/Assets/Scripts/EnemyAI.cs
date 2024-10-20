using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Image enemyHPBar;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator animator;

    [SerializeField] int enemyDifficulty;
    [SerializeField] int HP;
    [SerializeField] int rotateSpeed;
    [SerializeField] int sightLineAngle;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] int shootAngle;
    [SerializeField] float gravity;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] bool isLargeSpider;
    [SerializeField] GameObject miniSpiderPrefab; //smaller spider prefab
    [SerializeField] int numberOfMiniSpiders; //amount of spiders to spawn
    [SerializeField] float spawnRadius; 

    bool IsShooting;
    bool playerInRange;
    int originalEnemyHP;
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
        yield return new WaitForSeconds(roamPauseTime);

        agent.stoppingDistance = 0;
        Vector3 randomDistance = Random.insideUnitSphere * roamDistance;
        randomDistance += lastPosition;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDistance, out hit, roamDistance, 1);
        agent.SetDestination(hit.position);

        isRoaming = false;
        someCo = null;
    }

    void ApplyGravity()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded)
        {
            verticalVel = 0; // Reset vertical velocity when grounded
        }
        else
        {
            verticalVel += gravity * Time.deltaTime; // Apply gravity when not grounded
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
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightLineAngle)
            {

                float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (distanceToPlayer <= shootDistance)
                {
                    

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        faceTarget();
                    }

                    if (!IsShooting && angleToPlayer < shootAngle)
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
            agent.stoppingDistance = 0;
        }
    }

    IEnumerator shoot()
    {
        IsShooting = true;

        animator.SetTrigger("attackTrigger");
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);

        IsShooting = false;
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        updateEnemyUI();
        
        StartCoroutine(flashDamageColor());

        if(someCo != null)
        {
            StopCoroutine(someCo);
            isRoaming = false;
        }
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (HP <= 0)
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
        enemyHPBar.fillAmount = (float)HP / originalEnemyHP;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, shootDistance);

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

        if (isLargeSpider)
        {
            for (int i = 0; i < numberOfMiniSpiders; i++)
            {

                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPosition.y = transform.position.y; //keeping height of orignal spider

                Instantiate(miniSpiderPrefab, spawnPosition, Quaternion.identity);
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
            HP *= 2;
            shootRate *= 2;
            shootDistance *= 2;
            // increase move speed like full time sprint
        }
        else if(difficulty == 1)
        {
            HP /= 2;
            shootRate /= 2;
            shootDistance /= 2;
            // move speed stays at normal speed
        }
        return difficulty;
    }
}
