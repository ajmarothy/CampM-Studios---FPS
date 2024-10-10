using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour , IDamage
{
    [SerializeField] int HP;
    [SerializeField] GameObject switchObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
   
        if (HP <= 0)
        {
         
            Destroy(gameObject);
        }
    }
}
