using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour , IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [SerializeField] List<healthStats> healthInv = new List<healthStats>();
    [SerializeField] public int HP;
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
    [SerializeField] float reloadTime;

    public int originalPlayerHP;
    public int healthPickup;
    
    int healthPickupMax = 3;
    int selectedGunPos;
    int selectedHealthItem;
    int jumpCount;
    bool isShooting;
    bool isSprinting;
    bool isReloading;

    Vector3 moveDir;
    Vector3 playerVel;
    Coroutine flashreload;

    [SerializeField] Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        originalPlayerHP = HP;
        HealToFull();
        playerCamera.transform.localRotation = Quaternion.identity;
        updatePlayerUI();
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.GetPause())
        {
            movement();
            selectGun();
            if (Input.GetKeyDown(KeyCode.H))
            {
                UseHealth();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
        sprint();
    }

    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / originalPlayerHP;
        GameManager.instance.playerHPValue.text = (((float)HP / originalPlayerHP) * 100f).ToString("F0") + "%";
        GameManager.instance.healMax.text = healthPickupMax.ToString("F0");
        if (gunList.Count > 0)
        {
            GameManager.instance.ammoMax.text = gunList[selectedGunPos].ammoMax.ToString("F0");
            GameManager.instance.ammoCur.text = gunList[selectedGunPos].ammoCurr.ToString("F0");
        }
        if (healthInv.Count > 0)
        {
            GameManager.instance.healItem.text = healthInv.Count.ToString("F0");
        }
    }


    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.getSpawnPos().transform.position;
        controller.enabled = true;
        HP = originalPlayerHP;
        updatePlayerUI();
    }


    #region Player Controls
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
        Shooting();
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

    public void applyRecoil()
    {
        cameraController cameraController = playerCamera.GetComponent<cameraController>();
        cameraController.ApplyRecoil(recoilAmount);
    }

    #endregion

    #region Health
    public void takeDamage(int amount)
    {
        HP -= amount;
        HP = Mathf.Max(HP, 0);

        updatePlayerUI();
        StartCoroutine(damageFlash());
        if (HP <= 0)
        {
            GameManager.instance.YouLose();
        }
    }

    public void GetHealth(healthStats health)
    {
        healthInv.Add(health);
        selectedHealthItem = healthInv.Count - 1;
        updatePlayerUI();
    }

    public void UseHealth()
    {
        if (healthInv.Count > 0 && HP < originalPlayerHP)
        {
            healthStats health = healthInv[0];
            health.Heal(this);
            healthInv.RemoveAt(0);
            health.healItem--;
            Debug.Log("Used healing item: " + health.itemName);
            updatePlayerUI();
        }
        else if (healthInv.Count > 0 && HP == originalPlayerHP)
        {
            Debug.Log("Player does not need any health.");
        }
        else
        {
            Debug.Log("No healing items available.");
        }
    }

    public void HealToFull()
    {
        HP = originalPlayerHP;
        updatePlayerUI();
        Debug.Log("Player healed to full health");
    }

    public void Heal(int healAmount)
    {
        HP = Mathf.Min(HP + healAmount, originalPlayerHP);
        updatePlayerUI();
        Debug.Log("Player healed by " + healAmount);
    }
    #endregion

    #region Guns
    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        
        selectedGunPos = gunList.Count - 1;
        updatePlayerUI();

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDistance;
        shootRate = gun.shootRate;
        recoilAmount = gun.recoilAmount;
        reloadTime = gun.reloadTime;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGunPos < gunList.Count -1)
        {
            selectedGunPos++;
            updatePlayerUI();
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGunPos > 0)
        {
            selectedGunPos--;
            updatePlayerUI();
            changeGun();
        }
    }

    void changeGun()
    {
        updatePlayerUI();
        shootDamage = gunList[selectedGunPos].shootDamage;
        shootDist = gunList[selectedGunPos].shootDistance;
        shootRate = gunList[selectedGunPos].shootRate;
        recoilAmount = gunList[selectedGunPos].recoilAmount;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGunPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGunPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void Shooting()
    {
        if (Input.GetButton("Fire1") && !GameManager.instance.GetPause() && !isReloading && gunList.Count > 0 && gunList[selectedGunPos].ammoCurr > 0 && !isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    void Reload()
    {
        if (gunList.Count > 0)
        {
            gunStats currentGun = gunList[selectedGunPos];
            if (currentGun.ammoCurr < currentGun.ammoMax)
            {
                StartCoroutine(ReloadRoutine(currentGun));
            }
        }
    }

    #endregion

    #region IEnumerators
    IEnumerator shoot()
    {
        isShooting = true;
        gunList[selectedGunPos].ammoCurr--;
        updatePlayerUI();
        StartCoroutine(flashMuzzle());

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            Instantiate(gunList[selectedGunPos].hitEffect, hit.point, Quaternion.identity);
            if (dmg != null)
            {
                // add the Vector3.zero once directional damage is applied
                dmg.takeDamage(shootDamage);
            }
        }
        applyRecoil();
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator damageFlash()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScreen.SetActive(false);
    }

    IEnumerator flashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator ReloadRoutine(gunStats gun)
    {
        isShooting = false; // stop shooting
        isReloading = true;
        // add UI message for reloading (orange and flashed over reticle)
        if (flashreload != null)
        {
            StopCoroutine(flashreload);
            flashreload = null;
        }
        GameManager.instance.reloading.gameObject.SetActive(false);
        gunModel.SetActive(false);

        flashreload = StartCoroutine(FlashReload());
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);
        // reload / fill the ammo
        gun.ammoCurr = gun.ammoMax;

        StopCoroutine(FlashReload());
        flashreload = null;
        GameManager.instance.reloading.gameObject.SetActive(false);

        gunModel.SetActive(true);
        updatePlayerUI();
        isReloading = false;
    }

    IEnumerator FlashReload()
    {
        GameManager.instance.reloading.gameObject.SetActive(true);
        yield return new WaitForSeconds(reloadTime);
        GameManager.instance.reloading.gameObject.SetActive(false);
    }

    #endregion
}
