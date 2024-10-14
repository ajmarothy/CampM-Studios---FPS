using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{



    [SerializeField] Image enemyHPBar;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator animator;

    [SerializeField] int HP;
    [SerializeField] int rotateSpeed;
    [SerializeField] int sightLineAngle;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] float gravity;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;

    bool IsShooting;
    bool playerInRange;
    int originalEnemyHP;

    float angleToPlayer;
    float verticalVel;

    Vector3 playerDir;
    Vector3 lastPosition;

    Color colorOriginal;



    public int GetOGhp()
    {
        return originalEnemyHP;
    }

    public int GetHP()
    {
        return HP;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalEnemyHP = HP;
        updateEnemyUI();

        colorOriginal = model.material.color;
        GameManager.instance.updateGameGoal(1);
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        ApplyGravity();

        if (playerInRange && canSpotPlayer())
        {



        }
        enemyAnimation();


    }

    void ApplyGravity()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded)
        {
            verticalVel = 0; // Reset vertical velocity when grounded
        }
        else
        {
            verticalVel += gravity * Time.deltaTime; // Apply gravity when not grounded
        }

        // Move the agent with the vertical velocity
        Vector3 movement = new Vector3(0, verticalVel, 0);
        agent.Move(movement * Time.deltaTime);
    }


    bool canSpotPlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightLineAngle)
            {

                float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (distanceToPlayer <= shootDistance)
                {
                    

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        faceTarget();
                    }

                    if (!IsShooting)
                    {
                        StartCoroutine(shoot());
                    }
                }

                return true;
            }
        }
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotateSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        IsShooting = true;

        animator.SetTrigger("attackTrigger");
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);

        IsShooting = false;
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        updateEnemyUI();

        agent.SetDestination(GameManager.instance.player.transform.position);

        StartCoroutine(flashDamageColor());
        if (HP <= 0)
        {
            
            animator.SetTrigger("deathTrigger");

            StartCoroutine(waitForDeathAnimation());

            GameManager.instance.updateGameGoal(-1);
        }
    }

    IEnumerator flashDamageColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOriginal;
    }

    public void updateEnemyUI()
    {
        //GameManager.instance.enemyHPValue.text = (((float)HP / originalEnemyHP) * 100).ToString();
        enemyHPBar.fillAmount = (float)HP / originalEnemyHP;
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;


        Gizmos.DrawWireSphere(transform.position, shootDistance);


        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            Gizmos.color = Color.green;
            Vector3 playerPosition = GameManager.instance.player.transform.position;
            Gizmos.DrawLine(transform.position, playerPosition);
        }
    }

    void enemyAnimation()
    {

        if (Vector3.Distance(lastPosition, transform.position) > 0.005f)
        {
            animator.SetBool("isWalking", true);
            Debug.Log("Switching to walking animation");
        }
        else
        {
            animator.SetBool("isWalking", false);
            Debug.Log("Switching to idle animation");
        }

        lastPosition = transform.position;
    }

    IEnumerator waitForDeathAnimation()
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animatorStateInfo.length;

        yield return new WaitForSeconds(animationLength);

        Destroy(gameObject);
    }
}
