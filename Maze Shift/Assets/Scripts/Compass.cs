using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public GameObject icon;
    List<CompassMarker> compassMarkers = new List<CompassMarker>();

    public RawImage compassImage;
    public Transform player;

    //public float maxdistance = 50f;

    float compassUnit;

    public CompassMarker fountain;
    public CompassMarker healthStored;
    public CompassMarker healthInstant;
    public CompassMarker gun;
    public CompassMarker ammo;

    // Start is called before the first frame update
    void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;
        AddCompassMarker(fountain);
        AddCompassMarker(healthStored);
        AddCompassMarker(healthInstant);
        AddCompassMarker(gun);
        AddCompassMarker(ammo);
    }

    // Update is called once per frame
    void Update()
    {
        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        foreach (CompassMarker marker in compassMarkers)
        {
            marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

            float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.Position);
            float scale = 0f;

            if (distance < marker.maxDistance)
            {
                scale = 1f - (distance / marker.maxDistance);
            }

            if (distance > marker.maxDistance)
            {
                scale = 0f;
            }
             marker.image.rectTransform.localScale = Vector3.one * scale; //scales the image based on distance

            Color tempColor = marker.image.color;             // uses the scale to change the opacity or "fade in/out" the object 
            tempColor.a = 0.25f + scale;
            if (tempColor.a > 1)
                tempColor.a = 1;
            marker.image.color = tempColor;
        }
    }

    public void AddCompassMarker(CompassMarker marker)
    {
        GameObject newMarker = Instantiate(icon, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        compassMarkers.Add(marker);
    }

    public void DeleteCompassMarker(CompassMarker marker)
    {
        compassMarkers.Remove(marker);
        Destroy(marker);
    }

    Vector2 GetPosOnCompass(CompassMarker marker)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.Position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}
