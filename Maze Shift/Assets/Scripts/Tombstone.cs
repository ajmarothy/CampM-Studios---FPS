using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float speed = 10f;
    private Vector3 moveDirection;  

    // Start is called before the first frame update
    void Start()
    {
       
        if (moveDirection == Vector3.zero)
        {
            moveDirection = transform.forward;  
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
    }

    
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;  
    }
}
