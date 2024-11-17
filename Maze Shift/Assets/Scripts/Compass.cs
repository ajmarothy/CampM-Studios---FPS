using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public GameObject icon;
    List<GameObject> compassMarkers = new List<GameObject>();

    public RawImage compassImage;
    public Transform player;

    float compassUnit;

    GameObject[] unlistedCompassMark;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        compassUnit = compassImage.rectTransform.rect.width / 360f;

        unlistedCompassMark = GameObject.FindGameObjectsWithTag("Compass Marker");
        FillCompassMarkerList(unlistedCompassMark);

    }

    // Update is called once per frame
    void Update()
    {
        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        for (int i = 0; i < compassMarkers.Count; i++)
        {
            GameObject marker = compassMarkers[i];
            if (marker == null)
            {
                compassMarkers.Remove(marker);
                if (i > 0 && i < compassMarkers.Count - 1)
                {
                    i--;
                    continue;
                }
                else if (i == 0)
                    continue;
                else
                    break;
                //marker.GetComponent<CompassMarker>().DestroyCompassMarker();
            }
            if (marker.GetComponent<CompassMarker>().unwanted == true)
            {
                //Debug.Log("Delete Attempted");
                marker.GetComponent<CompassMarker>().image.rectTransform.localScale = Vector3.one * 0;
                compassMarkers.Remove(marker);
                if (i > 0 && i < compassMarkers.Count - 1)
                {
                    i--;
                    marker.GetComponent<CompassMarker>().DestroyCompassMarker();
                    continue;
                }
                    
                else if (i == 0)
                {
                    marker.GetComponent<CompassMarker>().DestroyCompassMarker();
                    continue;
                }
                    
                else
                {
                    marker.GetComponent<CompassMarker>().DestroyCompassMarker();
                    break;
                }
            }
           
            marker.GetComponent<CompassMarker>().image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

            float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.GetComponent<CompassMarker>().Position);
            float scale = 0f;
           
            if (distance < marker.GetComponent<CompassMarker>().maxDistance)
            {
                scale = 1f - (distance / marker.GetComponent<CompassMarker>().maxDistance);
            }

            if (distance > marker.GetComponent<CompassMarker>().maxDistance)
            {
                scale = 0f;
            }
            marker.GetComponent<CompassMarker>().image.rectTransform.localScale = Vector3.one * scale; //scales the image based on distance

            Color tempColor = marker.GetComponent<CompassMarker>().image.color;             // uses the scale to change the opacity or "fade in/out" the object 
            tempColor.a = 0.25f + scale;
            if (tempColor.a > 1)
                tempColor.a = 1;
            marker.GetComponent<CompassMarker>().image.color = tempColor;


        }


    }
    public void FillCompassMarkerList(GameObject[] compassMarkerList)
    {
        foreach (GameObject compassMarker in compassMarkerList)
        {
            AddCompassMarker(compassMarker);
        }
    }

    public void AddCompassMarker(GameObject marker)
    {
        GameObject newMarker = Instantiate(icon, compassImage.transform);                               // creates a game object on the compass and binds it to the compass image's transform                 
        marker.GetComponent<CompassMarker>().image = newMarker.GetComponent<Image>();                   // sets the marker object passed in to have the image as the object created above.
        marker.GetComponent<CompassMarker>().image.sprite = marker.GetComponent<CompassMarker>().icon;  // sets the sprite (aka visual texture) to be an image pre-set by the prefab marker used to appear on the compass

        compassMarkers.Add(marker);
    }

    //public void DeleteCompassMarker(GameObject marker)
    //{
        //compassMarkers.Remove(marker);
        //Destroy(gameObject);
    //}

    Vector2 GetPosOnCompass(GameObject marker)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.GetComponent<CompassMarker>().Position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}
