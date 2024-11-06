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

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        
        UpdateBossStage();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer = Time.deltaTime;

     

        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        UpdateBossStage();

        currHealth -= damage;
        currHealth = Mathf.Max(currHealth, 0);

        

        Debug.Log("Boss Health: " + currHealth + " | Stage: " +  currStage);

        if(currHealth <= 0)
        {
            BossKilled();
        }
    }

    private void UpdateBossStage()
    {
        if (currHealth <= 0) return;
   

        if(currHealth <= stage2Threshold && !stage2Activated)
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
        if(effect != null)
        {
            effect.SetActive(true);
        }
    }

    private void BossKilled()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
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
        if (attackTimer <= 0)
        {
            PhantomSlashAttack();
            attackTimer = attackCooldown;
        }

        if (attackTimer <= 0)
        {
            TombstoneBarrageAttack();
            attackTimer = attackCooldown;
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
        if (darkOrbAttack != null)
        {
            Instantiate(darkOrbAttack, transform.position, Quaternion.identity);
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
        if (tombstoneBarrageAttack != null)
        {
            Instantiate(tombstoneBarrageAttack, transform.position, Quaternion.identity);
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
}
