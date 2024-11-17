using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTrigger : MonoBehaviour
{
    public healthStats healthItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerScript.HP < GameManager.instance.playerScript.originalPlayerHP)
        {
            healthItem.Heal(GameManager.instance.playerScript);
            var receiver = GetComponentInChildren<ParentListener>();
            receiver.Notify(destroyed: true, unwanted: true);
            Destroy(gameObject);
        }
    }
}
