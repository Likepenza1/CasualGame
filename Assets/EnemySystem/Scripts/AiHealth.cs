using UnityEngine;

public class AiHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private NumberDisplay numberDisplay;
    public Animator animator;
    public CapsuleCollider2D colliderAi;
    public bool bDead = false;

    public int xpReward = 20;
    public int goldReward = 15;

    void Start()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Initialize(player.level);
        }

        numberDisplay = FindObjectOfType<NumberDisplay>();
    }
    
    public void Initialize(int playerLevel)
    {
        maxHealth = 50 + (playerLevel - 1) * 25;
        currentHealth = maxHealth;
        xpReward = 20 + (playerLevel - 1) * 10;
        goldReward = 15 + (playerLevel - 1) * 8;
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        bDead = true;

        animator.SetBool("Death", true);
        animator.SetBool("Move", false);

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.GainXP(xpReward);
            player.AddGold(goldReward);
        }
        
        AiSpawner aiSpawner = FindObjectOfType<AiSpawner>();
        if (aiSpawner != null)
        {
            aiSpawner.EnemyDestroyed();
        }

        if (numberDisplay != null)
        {
            numberDisplay.killEnemy++;
        }

        Destroy(colliderAi);
        Destroy(gameObject);
    }
}