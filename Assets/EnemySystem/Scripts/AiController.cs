using UnityEngine;

public class AiController : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    public int damage = 10;

    private Transform playerTransform;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;

    public bool Range = false;
    public GameObject arrowPrefab; // Префаб стрелы
    public Transform firePoint;    // Точка, откуда стрелять (например, позиция рук/лука)

    public AiHealth aiHealth;
    public Animator animator;

    public event System.Action<GameObject> OnMobDestroyed;

    void Start()
    {
        FindPlayer();

        if (playerTransform != null)
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                damage = 5 + (player.level - 1) * 2;
            }
        }
    }

    void Update()
    {
        if (aiHealth.bDead == false)
        {
            if (playerTransform == null)
            {
                FindPlayer();
                if (playerTransform == null)
                    return;
            }

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRange)
            {
                if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
                {
                    AttackPlayer();
                }
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        if (playerHealth.bDeath == false)
        {
            animator.SetBool("Move", true);
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    void AttackPlayer()
    {
        animator.SetBool("Move", false);
        isAttacking = true;
        lastAttackTime = Time.time;

        if (Range)
        {
            animator.SetTrigger("ShotBowAi");
            
        }
        else
        {
            animator.SetTrigger("Slash1H");

            PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
            PlayerController playerController = playerTransform.GetComponent<PlayerController>();

            if (playerHealth != null)
            {
                int scaledDamage = damage;
                if (playerController != null)
                {
                    int playerLevel = playerController.level;
                    scaledDamage = Mathf.CeilToInt(damage * (1 + (playerLevel - 1) * 0.2f));
                }

                playerHealth.TakeDamage(scaledDamage);
            }
        }

        Invoke("ResetAttack", attackCooldown);
    }
    
    public void BowSootAi()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            arrow.GetComponent<ProjectileMover>().SetTarget(playerTransform, damage);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void OnDestroy()
    {
        OnMobDestroyed?.Invoke(gameObject);
    }
}