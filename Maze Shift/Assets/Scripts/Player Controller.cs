using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour , IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [SerializeField] List<healthStats> healthInv = new List<healthStats>();
    [SerializeField] int HP;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int jumpMax;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] float recoilAmount;


    int selectedGunPos;
    int jumpCount;
    bool isShooting;
    bool isSprinting;
    int originalPlayerHP;
    int healthPickup;

    Vector3 moveDir;
    Vector3 playerVel;

    [SerializeField] Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        originalPlayerHP = HP;
        playerCamera.transform.localRotation = Quaternion.identity;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused) 
        {
            movement();
            selectGun();
        }
        
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

        if (Input.GetButton("Fire1") && !GameManager.instance.isPaused && !isShooting && gunList.Count > 0 && gunList[selectedGunPos].ammoCurr > 0)
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

        gunList[selectedGunPos].ammoCurr--;
        StartCoroutine(flashMuzzel());

       
       

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            Instantiate(gunList[selectedGunPos].hitEffect, hit.point, Quaternion.identity);

            if(dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

        applyRecoil();

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
        
    }

    IEnumerator flashMuzzel()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
    }

    public void applyRecoil()
    {
        cameraController cameraController = playerCamera.GetComponent<cameraController>();
        cameraController.ApplyRecoil(recoilAmount);
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(damageFlash());
        if(HP <= 0)
        {
            GameManager.instance.YouLose();
        }
    }

    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / originalPlayerHP;
        GameManager.instance.playerHPValue.text = (((float)HP / originalPlayerHP) * 100f).ToString("F0") + "%";
    }

    IEnumerator damageFlash()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScreen.SetActive(false);
    }

    public void GetHealth(healthStats health)
    {
        healthInv.Add(health);
        updatePlayerUI();
    }

    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        selectedGunPos = gunList.Count - 1;

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDistance;
        shootRate = gun.shootRate;
        recoilAmount = gun.recoilAmount;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGunPos < gunList.Count -1)
        {
            selectedGunPos++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGunPos > 0)
        {
            selectedGunPos--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootDamage = gunList[selectedGunPos].shootDamage;
        shootDist = gunList[selectedGunPos].shootDistance;
        shootRate = gunList[selectedGunPos].shootRate;
        recoilAmount = gunList[selectedGunPos].recoilAmount;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGunPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGunPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
