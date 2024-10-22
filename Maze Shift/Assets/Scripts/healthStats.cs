using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class healthStats : ScriptableObject
{
    public GameObject health;
    public string itemName; 
    public int healthAmount; // actual HP added to health, will never exceed 100%
    public int healItem, healItemMax; // number of pickups
    public float healthPercentage; // a float between 0.00 and 1.00
    public bool isMaxHeal; 

    public void Heal(PlayerController player)
    {
        if (isMaxHeal)
        {
            player.HealToFull();
        }
        else
        {
            healthAmount = Mathf.RoundToInt(player.originalPlayerHP * healthPercentage);
            player.Heal(healthAmount);
        }
    }
}
