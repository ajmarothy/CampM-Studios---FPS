using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunStats : ScriptableObject
{
    // Max, feel free to change this however you want. I needed to create this script so I could also create the pickup script for heal

    public GameObject gunModel;
    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;

    public int shootDamage;
    public int shootDistance;
    public int ammoCurr, ammoMax, ammoPickup;
    public float shootRate;
    public float reloadTime;
    public float shootVol;
}
