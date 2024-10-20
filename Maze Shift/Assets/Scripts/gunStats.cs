using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{

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
