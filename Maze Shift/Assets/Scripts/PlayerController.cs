using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour , IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [SerializeField] int HP;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int jumpMax;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    
    
    int jumpCount;
    bool isShooting;
    bool isSprinting;


    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
    }


    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if(Input.GetButton("Fire1") &&!isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if(dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
        
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
    }
}
