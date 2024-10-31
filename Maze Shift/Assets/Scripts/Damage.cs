using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] public enum damageType { bullet, chaser, stationary, fireBall, lobbed}
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        if(type == damageType.bullet || type == damageType.fireBall)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
        else if(type == damageType.chaser)
        {
            Destroy(gameObject, destroyTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }
        if (type == damageType.bullet || type == damageType.chaser || type == damageType.lobbed)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == damageType.chaser)
        {
            rb.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    public void Initialize(Vector3 target, damageType damageType)
    {
        targetPos = target;
        type = damageType;

        if (type == damageType.lobbed)
        {
            StartCoroutine(LobProjectile());
        }
        else
        {
            Destroy(gameObject, destroyTime);
        }
    }

    private IEnumerator LobProjectile()
    {
        Vector3 startPosition = transform.position;
        float height = Vector3.Distance(startPosition, targetPos) / 2; //height for lob effect

        float elapsedTime = 0f;
        float duration = 1.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float yOffset = height * Mathf.Sin(t * Mathf.PI); //creates the lob effect
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPos, t);
            newPosition.y += yOffset; //apply the lob effect

            transform.position = newPosition;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPos;
        HandleCollision(null);
    }

    private void HandleCollision(Collider other)
    {
        IDamage dmg = other?.GetComponent<IDamage>(); //check if the collider is not null
        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }

        Destroy(gameObject); //destroy the projectile
    }
}
