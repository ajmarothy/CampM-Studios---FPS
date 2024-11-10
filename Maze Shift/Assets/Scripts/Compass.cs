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

    // Start is called before the first frame update
    void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;

        AddCompassMarker(fountain);
    }

    // Update is called once per frame
    void Update()
    {
        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        foreach (CompassMarker marker in compassMarkers)
        {
            marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

            float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.position);
            float scale = 0f;

            if (distance < marker.maxDistance)
            {
                scale = 1f - (distance / marker.maxDistance);
            }

            marker.image.rectTransform.localScale = Vector3.one * scale;
        }
    }

    public void AddCompassMarker(CompassMarker marker)
    {
        GameObject newMarker = Instantiate(icon, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        compassMarkers.Add(marker);
    }

    Vector2 GetPosOnCompass(CompassMarker marker)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}
