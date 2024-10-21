using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class healthStats : ScriptableObject
{
    public GameObject health;
    public AudioClip[] healSound;

    public int healValue;
    public float healVolume;
}
