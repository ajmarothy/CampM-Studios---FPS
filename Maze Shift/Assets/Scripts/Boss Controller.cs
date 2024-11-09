using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Health and Stages")]
    [SerializeField] private int maxHealth = 300;
    private int currHealth;
    public int stage2Threshold;
    public int stage3Threshold;
    private int currStage = 1;

    [Header("Stage Effects")]
    [SerializeField] private GameObject stage2Effect;
    [SerializeField] private GameObject stage3Effect;

    [Header("Attacks")]
    [SerializeField] private float attackCooldown = 3f;

    //stage 1 attacks
    [SerializeField] private GameObject darkOrbAttack;
    [SerializeField] private GameObject tombSummonAttack;
    [SerializeField] private GameObject trapActivationAttack;
    //stage 2 attacks
    [SerializeField] private GameObject phantomSlashAttack;
    [SerializeField] private GameObject tombstoneBarrageAttack;
    //stage 3 attacks
    [SerializeField] private GameObject cursedWhirlwindAttack;
    [SerializeField] private GameObject soulLeechAttack;
    [SerializeField] private GameObject artifactExplosionAttack;

    private bool stage2Activated = false;
    private bool stage3Activated = false;

    private float attackTimer = 0f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectionRange = 10f;
    private Transform playerPos;

    private float currStoppingDist = 5f;

    private bool isBarrageActive = false;

    private float fixedY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        playerPos = GameManager.instance.player.transform;
        UpdateBossStage();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (Vector3.Distance(transform.position, playerPos.position) <= detectionRange)
        {
            MoveToPlayer();
        }


        if (currStage == 1 && attackTimer <= 0)
        {
            Stage1Attacks();
        }
        else if (currStage == 2 && attackTimer <= 0)
        {
            Stage2Attacks();
        }
        else if (currStage == 3 && attackTimer <= 0)
        {
            Stage3Attacks();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(10);
        }
    }

    private void MoveToPlayer()
    {
        if (playerPos != null)
        {
            float distToPlayer = Vector3.Distance(transform.position, playerPos.position);


            if (distToPlayer > currStoppingDist)
            {
                Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
                Vector3 newPosition = Vector3.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);
                newPosition.y = fixedY;

                transform.position = newPosition;

                Quaternion targetRot = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        UpdateBossStage();

        currHealth -= damage;
        currHealth = Mathf.Max(currHealth, 0);



        Debug.Log("Boss Health: " + currHealth + " | Stage: " + currStage);

        if (currHealth <= 0)
        {
            BossKilled();
        }
    }

    private void UpdateBossStage()
    {
        if (currHealth <= 0) return;


        if (currHealth <= stage2Threshold && !stage2Activated)
        {
            stage2Activated = true;
            currStage = 2;
            Debug.Log("Stage 2 Activated");
            ActivateStageEffect(stage2Effect);
        }

        else if (currHealth <= stage3Threshold && !stage3Activated)
        {
            stage3Activated = true;
            currStage = 3;
            Debug.Log("Stage 3 Activated");
            ActivateStageEffect(stage3Effect);
        }
    }

    private void ActivateStageEffect(GameObject effect)
    {
        if (effect != null)
        {
            effect.transform.SetParent(transform);

            effect.transform.localPosition = new Vector3(0, 2, 0);

            effect.SetActive(true);
        }
    }

    private void BossKilled()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }

    private void SetStoppingDistForAttack(string attackType)
    {
        switch (attackType)
        {
            case "melee":
                currStoppingDist = 2f;
                break;
            case "ranged":
                currStoppingDist = 6f;
                break;
            default:
                currStoppingDist = 5f;
                break;
        }
    }

    #region Attack Stages 

    private void Stage1Attacks()
    {
        if (attackTimer <= 0)
        {
            DarkOrbAttack();
            attackTimer = attackCooldown;
        }

        if (attackTimer <= 0)
        {
            TombSummonAttack();
            attackTimer = attackCooldown;
        }

        if (attackTimer <= 0)
        {
            TrapActivation();
            attackTimer = attackCooldown;
        }
    }

    private void Stage2Attacks()
    {
        if (!isBarrageActive)
        {
            isBarrageActive = true;
            TombstoneBarrageAttack();
        }


    }

    private void Stage3Attacks()
    {
        if (attackTimer <= 0)
        {
            CursedWhirlwindAttack();
            attackTimer = attackCooldown;
        }

        if (attackTimer <= 0)
        {
            SoulLeechAttack();
            attackTimer = attackCooldown;
        }

        if (currHealth <= 10)
        {
            ArtifactExplosion();
        }
    }


    #endregion

    #region Stage 1 Attacks Types 

    private void DarkOrbAttack()
    {
        SetStoppingDistForAttack("ranged");

        if (darkOrbAttack != null)
        {
            GameObject orb = Instantiate(darkOrbAttack, transform.position, Quaternion.identity);

            Rigidbody orbRb = orb.GetComponent<Rigidbody>();
            if (orbRb != null)
            {
                Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
                orbRb.velocity = directionToPlayer * 10f;
            }

            Debug.Log("Boss uses Dark Orb!");
        }
    }

    private void TombSummonAttack()
    {
        if (tombSummonAttack != null)
        {
            Instantiate(tombSummonAttack, transform.position, Quaternion.identity);
            Debug.Log("Boss summons undead minions!");
        }
    }

    private void TrapActivation()
    {
        if (trapActivationAttack != null)
        {
            Instantiate(trapActivationAttack, transform.position, Quaternion.identity);
            Debug.Log("Boss activates traps!");
        }
    }

    #endregion

    #region Stage 2 Attacks Types  
    private void PhantomSlashAttack()
    {
        if (phantomSlashAttack != null)
        {
            Instantiate(phantomSlashAttack, transform.position, Quaternion.identity);
            Debug.Log("Boss uses Phantom Slash!");
        }
    }

    private void TombstoneBarrageAttack()
    {
        if (tombstoneBarrageAttack != null && attackTimer <= 0)
        {
            Debug.Log("Tombstone Barrage triggered!");

            int numTombstones = 5;
            float spawnRadius = 3f;

            StartCoroutine(SpawnTombstones(numTombstones, spawnRadius));

            attackTimer = attackCooldown;

            Debug.Log("Boss uses Tombstone Barrage!");
        }
    }

    #endregion

    #region Stage 3 Attacks Types  
    private void CursedWhirlwindAttack()
    {
        if (cursedWhirlwindAttack != null)
        {
            Instantiate(cursedWhirlwindAttack, transform.position, Quaternion.identity);
            Debug.Log("Boss uses Cursed Whirlwind!");
        }
    }

    private void SoulLeechAttack()
    {
        if (soulLeechAttack != null)
        {
            Instantiate(soulLeechAttack, transform.position, Quaternion.identity);
            Debug.Log("Boss uses Soul Leech!");
        }
    }

    private void ArtifactExplosion()
    {
        if (artifactExplosionAttack != null)
        {
            Instantiate(artifactExplosionAttack, transform.position, Quaternion.identity);
            Debug.Log("Boss uses Artifact Explosion!");
        }
    }

    #endregion

    private IEnumerator SpawnTombstones(int numTombstones, float spawnRadius)
    {
        Debug.Log("Starting Tombstone Barrage Coroutine");

        for (int i = 0; i < numTombstones; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));

            GameObject tombstone = Instantiate(tombstoneBarrageAttack, spawnPos, Quaternion.identity);

            Tombstone tombstoneScript = tombstone.GetComponent<Tombstone>();

            
            if (tombstoneScript != null)
            {
               
                Vector3 randomDirection = (transform.forward + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f))).normalized;

                
                tombstoneScript.SetDirection(randomDirection);
            }

           
            Destroy(tombstone, 3f);
        }
        yield return null;
        isBarrageActive = false;
        Debug.Log("Tombstone Barrage complete!");
    }
}
