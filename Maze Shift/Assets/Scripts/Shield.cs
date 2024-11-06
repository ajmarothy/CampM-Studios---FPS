using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using UnityEditor.Build;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int shieldMaxHealth = 30;
    [SerializeField] private int shieldCurrHealth;
    [SerializeField] private float shieldRegenDelay = 1f;
    [SerializeField] private float shieldRegenRate = 10f;

    bool isRegenerating = false;
    private bool isActive = false;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        shieldCurrHealth = 0;
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive && shieldCurrHealth < shieldMaxHealth && !isRegenerating)
        {
            StartCoroutine(RegenerateShield());
        }
    }

    public void TakeDamage(int damage)
    {
        if (shieldCurrHealth > 0)
        {
            int damageToShield = Mathf.Min(damage, shieldCurrHealth);
            shieldCurrHealth -= damageToShield;

            
            int remainingDamage = damage - damageToShield;
            if (remainingDamage > 0)
            {
                playerController.takeDamage(remainingDamage);
            }
        }
        else
        {
            playerController.takeDamage(damage);
        }

       
        StopCoroutine(RegenerateShield());
        isRegenerating = false;
    }

    private IEnumerator RegenerateShield()
    {
        Debug.Log("Waiting before regeneration starts.");
        yield return new WaitForSeconds(shieldRegenDelay);
        isRegenerating = true;

        while (shieldCurrHealth < shieldMaxHealth)
        {
            shieldCurrHealth += (int)(shieldRegenRate * Time.deltaTime);
            shieldCurrHealth = Mathf.Min(shieldCurrHealth, shieldMaxHealth);
            yield return null;
        }

        isRegenerating = false;
        Debug.Log("Shield regeneration complete.");
    }

    public void StartRegeneration()
    {
        Debug.Log("Shield regeneration called.");
        isActive = true;
        if (!isRegenerating)
        {
            StartCoroutine(RegenerateShield());
        }
    }

    public int GetCurrentShieldHealth()
    {
        return shieldCurrHealth;
    }

    public int GetShieldMaxHealth()
    {
        return shieldMaxHealth;
    }

}
