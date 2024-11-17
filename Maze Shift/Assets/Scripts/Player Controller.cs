using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour , IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] Camera playerCamera;

    [SerializeField] List<healthStats> healthInv = new List<healthStats>();
    [SerializeField] public int HP;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int jumpMax;

    [SerializeField] private Shield playerShield;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] public gunStats currentGun;
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Light flashlight;
    [SerializeField] float maxBattery;
    [SerializeField] float batteryDrainRate;
    [SerializeField] float flickerThreshold;
    [SerializeField] float rechargeSpeed;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] float recoilAmount;
    [SerializeField] float reloadTime;

    public AudioSource audioSource;
    public AudioClip shootSound;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip sprintSound;
    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip flashlightSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip jumpSound;

    public int originalPlayerHP;
    public int healthPickup;

    int healthPickupMax = 3;
    int selectedGunPos;
    int selectedHealthItem;
    int jumpCount;
    float currentBattery;
    public float dwellTime = 5f;

    public bool isDialogActive = false;
    bool isFlickering;
    bool isRecharging;
    bool isShooting;
    bool isSprinting;
    bool isReloading;
    bool isWalking;

    Vector3 moveDir;
    Vector3 playerVel;
    Coroutine flashreload;
    Coroutine flicker;

    // Start is called before the first frame update
    void Start()
    {
        originalPlayerHP = HP;
        playerShield = null;
        playerCamera.transform.localRotation = Quaternion.identity;
        if(flashlight != null)
        {
            flashlight.enabled = false;
            isFlickering = false;
            isRecharging = false;
            GameManager.instance.batteryUI.material.color = Color.green;
        }
        currentBattery = maxBattery;
        updatePlayerUI();
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.GetPause())
        {
            movement();
            HandleWalkingSound();
            selectGun();
            UpdateWeaponRotation();

            if (Input.GetButtonDown("Heal"))
            {
                UseHealth();
                if (healthInv.Count == 0)
                {
                    GameManager.instance.healItem.text = healthInv.Count().ToString();
                    updatePlayerUI();
                }
            }
            if (Input.GetButtonDown("Reload"))
            {
                Reload();
            }
            if (Input.GetButtonDown("Flashlight"))
            {
                audioSource.PlayOneShot(flashlightSound);
                flashlight.enabled = !flashlight.enabled;
            }
            DrainPower();
            updatePlayerUI();
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
            GameManager.instance.ammoCur.text = gunList[selectedGunPos].ammoCurr.ToString("F0");
            GameManager.instance.ammoTotal.text = gunList[selectedGunPos].totalAmmo.ToString("F0");
        }
        if (healthInv.Count > 0)
        {
            GameManager.instance.healItem.text = healthInv.Count.ToString("F0");
        }
        GameManager.instance.batteryUI.fillAmount = (currentBattery / maxBattery);
        GameManager.instance.currentBattery.text = currentBattery.ToString("F0") + "%";
        GameManager.instance.maxBattery.text = maxBattery.ToString("F0") + "%";
        if(currentBattery == maxBattery) { GameManager.instance.batteryUI.material.color = Color.green; }
        if(currentBattery < maxBattery - (maxBattery * 0.4)) { GameManager.instance.batteryUI.material.color = Color.yellow; }
        if(currentBattery < flickerThreshold) { GameManager.instance.batteryUI.material.color = Color.red; }
        if(playerShield != null)
        {
            float shieldHealthPercent = (float)playerShield.GetCurrentShieldHealth() / playerShield.GetShieldMaxHealth();
            GameManager.instance.shieldHealthBar.fillAmount = shieldHealthPercent;

            // Display shield health as a percentage (e.g., "50%")
            GameManager.instance.shieldHealthValue.text = (shieldHealthPercent * 100f).ToString("F0") + "%";
        }
    }

    #region Flashlight
    public void DrainPower()
    {
        if(flashlight.enabled && currentBattery > 0)
        {
            currentBattery -= (batteryDrainRate * Time.deltaTime);
            updatePlayerUI();
            if (currentBattery <= flickerThreshold && !isFlickering)
            {
                isFlickering = true;
                StartCoroutine(FlickerFlashlight());
            }
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                flashlight.enabled = false;
                isFlickering = false;
            }
        }
    }

    public void StartRecharge()
    {
        if (!isRecharging)
        {
            isRecharging = true;
            StartCoroutine(RechargeBattery());
        }
    }
    #endregion

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
            audioSource.PlayOneShot(jumpSound);
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
        CameraController cameraController = playerCamera.GetComponent<CameraController>();
        cameraController.ApplyRecoil(recoilAmount);
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.getSpawnPos().transform.position;
        controller.enabled = true;
        HP = originalPlayerHP;
        updatePlayerUI();
    }

    void HandleWalkingSound()
    {
        bool isMoving = moveDir.magnitude > 0.1f && IsGrounded();

        if (isMoving && !isWalking && !isSprinting)
        {
            isWalking = true;
            isSprinting = false;
            if (audioSource.clip != walkSound || !audioSource.isPlaying)
            {
                audioSource.clip = walkSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (isMoving && Input.GetButton("Sprint") && !isSprinting)
        {
            isSprinting = true;
            isWalking = false;
            if (audioSource.clip != sprintSound || !audioSource.isPlaying)
            {
                audioSource.clip = sprintSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (!isMoving && (isWalking || isSprinting))
        {
            isWalking = false;
            isSprinting = false;
            audioSource.Stop();
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    #endregion

    #region Health
    public void takeDamage(int amount)
    {
        audioSource.PlayOneShot(takeDamageSound);

        if (playerShield != null && playerShield.GetCurrentShieldHealth() > 0)
        {
            playerShield.TakeDamage(amount);
        }
        else
        {
            HP -= amount;
            HP = Mathf.Max(HP, 0);

            updatePlayerUI();
            StartCoroutine(damageFlash());
            if (HP <= 0)
            {
                audioSource.PlayOneShot(deathSound);
                GameManager.instance.YouLose();
            }
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
            health.healItem--;
            StartCoroutine(FlashHeal());
            healthInv.RemoveAt(0);
            Debug.Log("Used healing item: " + health.itemName);
            updatePlayerUI();
        }
        else if (healthInv.Count >= 0 && HP == originalPlayerHP)
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
        StartCoroutine(FlashHeal());
        updatePlayerUI();
        Debug.Log("Player healed by " + healAmount);
        StopCoroutine(FlashHeal());
    }
    #endregion

    #region Shield

    public void PickupShield(Shield shield)
    {
        if (playerShield == null)
        {
            playerShield = shield;
            playerShield.StartRegeneration();

            GameManager.instance.ToggleShieldUI(true);
            Debug.Log("Shield picked up!");
        }
        else
        {
            Debug.Log("Player already has a shield!");
        }
    }

    #endregion

    #region Weapons
    public void getGunStats(gunStats gun)
    {
        gun.totalAmmo = Mathf.Min(gun.startingAmmo, gun.maxAmmoCapacity);
        if (!gunList.Contains(gun))
        {
            gunList.Add(gun);
        }
        
        selectedGunPos = gunList.Count - 1;
        updatePlayerUI();

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDistance;
        shootRate = gun.shootRate;
        recoilAmount = gun.recoilAmount;
        reloadTime = gun.reloadTime;
        gunList[selectedGunPos].totalAmmo = gun.totalAmmo;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectGun()
    {
        if (gunList.Count > 0)
        {
            currentGun = gunList[selectedGunPos];
            Debug.Log("Current gun: " + currentGun.name);
        }
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
        if (Input.GetButton("Fire1") && !GameManager.instance.GetPause() && !isReloading && gunList.Count > 0 && gunList[selectedGunPos].ammoCurr > 0 && !isShooting && !isDialogActive)
        {
            StartCoroutine(shoot());
            audioSource.PlayOneShot(shootSound);
        }
    }

    public List<gunStats> GetGunInventory()
    {
        return gunList;
    }

    void Reload()
    {
        if (!isReloading)
        {
            StartCoroutine(ReloadRoutine(gunList[selectedGunPos]));
        }
    }

    public void AddAmmo(gunStats gun, int ammoToAdd)
    {
        gun.totalAmmo += ammoToAdd;
        updatePlayerUI();
    }

    void UpdateWeaponRotation()
    {
        if (currentGun != null && currentGun.name == "Shovel")
        {
            Debug.Log("Shovel is equipped, applying rotation.");
            gunModel.transform.localRotation = Quaternion.Euler(-8.537f, 82.102f, -74.64f);
        }
        else
        {
            gunModel.transform.localRotation = Quaternion.identity;
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

            if (hit.collider.CompareTag("Boss"))
            {
                BossController bossController = hit.collider.GetComponent<BossController>();
                if (bossController != null)
                {
                    bossController.TakeDamage(shootDamage);
                    
                }
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
        if (isReloading || gun.ammoCurr == gun.ammoPerMag || gunList[selectedGunPos].totalAmmo == 0)
            yield break;

        isReloading = true;
        audioSource.PlayOneShot(reloadSound);
        gunModel.SetActive(false);
        GameManager.instance.reloading.gameObject.SetActive(true);
        int roundsNeeded = gun.ammoPerMag - gun.ammoCurr;
        gun.ammoCurr = 0;
        if(gunList[selectedGunPos].totalAmmo >= roundsNeeded)
        {
            gun.ammoCurr = gun.ammoPerMag;
            gunList[selectedGunPos].totalAmmo -= roundsNeeded;
        }
        else
        {
            gun.ammoCurr = gunList[selectedGunPos].totalAmmo;
            gunList[selectedGunPos].totalAmmo = 0;
        }
        yield return new WaitForSeconds(reloadTime);
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

    IEnumerator FlashHeal()
    {
        GameManager.instance.healingMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        GameManager.instance.healingMessage.gameObject.SetActive(false);
    }

    IEnumerator FlickerFlashlight()
    {
        while(isFlickering && flashlight.enabled)
        {
            flashlight.enabled = Random.value > 0.5f;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            flashlight.enabled = true;
        }
        if(currentBattery == 0) { flashlight.enabled = false; StartCoroutine(RechargeBattery()); }
    }

    IEnumerator RechargeBattery()
    {
        while (currentBattery < maxBattery && !flashlight.enabled)
        {
            dwellTime -= 1 * Time.deltaTime;
            if (dwellTime <= 0)
            {
                dwellTime = 0;
                currentBattery += rechargeSpeed * Time.deltaTime;
                updatePlayerUI();
            }
            if(currentBattery >= maxBattery)
            {
                currentBattery = maxBattery;
                isRecharging = false;
                dwellTime = 5f;
                yield break;
            }
            updatePlayerUI();
            
            yield return null;
        }
    }

    #endregion
}

