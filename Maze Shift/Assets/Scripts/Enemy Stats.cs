using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    //public int enemyDifficulty;
    public int HPStart;
    public int HPCurrent;
    public GameObject bullet;
    public float shootRate;
    public float shootDistance;
    public int shootAngle;
    public int rotateSpeed;
    public int sightLineAngle;
    public int roamDistance;
    public float roamPauseTime;
    public float gravity;
    public float groundCheckDistance;
    public LayerMask groundLayer;

    public bool isLargeSpider;
    public GameObject miniSpiderPrefab;
    public int numberOfMiniSpiders;
    public float spawnRadius;

    public enum AttackType { Bullet, Chaser, Lobbed }
    public AttackType attackType; 

    public int damageAmount; //damage dealt by the enemy
    public float speed; //speed for chaser or bullet

    public AudioClip attackSound;
    public AudioClip damageTakenSound;
    public AudioClip deathSound;
}
