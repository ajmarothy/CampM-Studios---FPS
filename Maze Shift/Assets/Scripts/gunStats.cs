using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public GameObject gunModel;
    public GameObject ammoPickup;
    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;

    public int shootDamage;
    public int shootDistance;
    public int ammoCurr, ammoPerMag, totalAmmo;
    public float shootRate;
    public float recoilAmount;
    public float reloadTime;
    public float shootVol;

}
