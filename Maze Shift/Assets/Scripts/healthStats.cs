using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class healthStats : ScriptableObject
{
    public GameObject health;
    public string itemName;
    public int healthAmount;
    public int healItem, healItemMax;
    public float healthPercentage;
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
