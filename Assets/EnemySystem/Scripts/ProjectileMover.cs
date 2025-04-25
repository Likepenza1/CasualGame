using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public GameObject Hit;
    public GameObject damageTextPrefab;

    public PlayerController Owner { get; private set; }

    private Transform target;
    private int damage;
    private bool isEnemyProjectile = false;
    public float speed = 10f;

    public void SetOwner(PlayerController owner)
    {
        Owner = owner;
    }
    
    public void SetTarget(Transform targetTransform, int damageAmount)
    {
        target = targetTransform;
        damage = damageAmount;
        isEnemyProjectile = true;
    }

    void Update()
    {
        if (isEnemyProjectile && target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                HitPlayer();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEnemyProjectile && collision.gameObject.CompareTag("Ai"))
        {
            HandleHit(collision.gameObject);
        }
    }

    void HandleHit(GameObject target)
    {
        if (Hit != null)
        {
            Instantiate(Hit, transform.position, Quaternion.identity);
        }

        AiHealth aiHealth = target.GetComponent<AiHealth>();
        if (aiHealth != null)
        {
            int totalDamage = Owner.GetTotalDamage();
            

            bool isCrit = Random.value < Owner.critChance;
            if (isCrit)
            {
                totalDamage = Mathf.CeilToInt(totalDamage * Owner.critMultiplier);
            }

            aiHealth.TakeDamage(totalDamage);

            if (damageTextPrefab != null)
            {
                GameObject dmgTextObj = Instantiate(damageTextPrefab, target.transform.position + Vector3.up * 1f, Quaternion.identity);
                DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
                if (dmgText != null)
                {
                    dmgText.Setup(totalDamage, isCrit);
                    dmgText.DestroyAfter(1.5f);
                }
            }

            if (aiHealth.bDead && Owner != null)
            {
                Owner.GainXP(aiHealth.xpReward);
                Owner.AddGold(aiHealth.goldReward);
            }
        }

        Destroy(gameObject);
    }

    void HitPlayer()
    {
        if (Hit != null)
        {
            Instantiate(Hit, transform.position, Quaternion.identity);
        }

        if (target != null)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
