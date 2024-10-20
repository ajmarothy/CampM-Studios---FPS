using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthStats : ScriptableObject
{
    public GameObject health;
    public GameObject healthPlus;
    public GameObject healthHeart;
    public AudioClip[] healSound;

    public int playerHP;
    public int maxHP;
    public int plusValue;
    public int heartValue;
    public int shootDistance;
    public int healthCurr, healthMax, healthPickup;
    public float healVolume;
}
