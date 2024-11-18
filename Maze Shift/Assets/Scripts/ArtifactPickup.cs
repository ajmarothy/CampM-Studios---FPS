using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    public enum ArtifactTypes { filler, firstLevel, secondLevel, thirdLevel, fourthLevel }

    public ArtifactTypes ArtifactType;

    public bool rotate;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate == true)
        {
            transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CollectArtifact();
            Destroy(gameObject);
        }
    }

    public void CollectArtifact()
    {
         //var collect = gameObject.GetComponentInParent<ArtifactCollected>();
        switch (ArtifactType)
        {
            case ArtifactTypes.filler:         // exists as a default or future use for another similar pickup method
                return;

            case ArtifactTypes.firstLevel:     // code for collecting each levels artifact goes in the switch statement. 
                Debug.Log("First Level Artifact Collected");
                gameObject.GetComponentInParent<ArtifactCollected>().ItemCollected();
                return; 

            case ArtifactTypes.secondLevel:
                Debug.Log("Second Level Artifact Collected");
                gameObject.GetComponentInParent<ArtifactCollected>().ItemCollected();
                return;

            case ArtifactTypes.thirdLevel:
                Debug.Log("Third Level Artifact Collected");
                gameObject.GetComponentInParent<ArtifactCollected>().ItemCollected();
                return;

            case ArtifactTypes.fourthLevel:
                Debug.Log("Fourth Level Artifact Collected");
                gameObject.GetComponentInParent<ArtifactCollected>().ItemCollected();
                return;
        }
    }
}
