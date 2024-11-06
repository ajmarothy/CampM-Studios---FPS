using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 300;
    private int currHealth;
    public int stage2Threshold;
    public int stage3Threshold;

    private int currStage = 1;

    private bool stage2Activated = false;
    private bool stage3Activated = false;

    [SerializeField] private GameObject stage2Effect;
    [SerializeField] private GameObject stage3Effect;


    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        
        UpdateBossStage();
    }

    // Update is called once per frame
    void Update()
    {
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

}
