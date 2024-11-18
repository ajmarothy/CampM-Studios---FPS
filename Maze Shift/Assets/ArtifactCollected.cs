using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactCollected : MonoBehaviour
{
    public bool collected = false;

    public GameObject endPortal;

    public void ItemCollected()
    {
        collected = true;
        endPortal.GetComponent<ArtifactCollected>().SetCollectedStatus(collected);
        Debug.Log("Artifact has been collected for the level");
    }

    public bool GetCollectedStatus() { return collected; }

    public void SetCollectedStatus(bool collectToF) { collected = collectToF; }

}
