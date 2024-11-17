using UnityEngine;
using UnityEngine.UI;

public class CompassMarker : MonoBehaviour
{
    public Sprite icon;
    public Image image;

    public float maxDistance;

    public bool unwanted = false;
    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.z); }
    }

    public void DestroyCompassMarker()
    {
        //Debug.Log("Destroying Compass Marker");
        Destroy(gameObject);
    }
}
