using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentListener : MonoBehaviour
{
    public void Notify( bool destroyed, bool unwanted)
    {
        if (destroyed)
        {
            transform.SetParent(null);
        }

        if (unwanted)
        {
            //Debug.Log("Object is unwanted");
            //Destroy(gameObject);
            this.GetComponent<CompassMarker>().unwanted = true;
            //Debug.Log("Destroying Object");
            //Destroy(gameObject);
        }
    }
}
