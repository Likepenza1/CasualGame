using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 500; // увеличено для баланса с уроном врагов
    public int currentHealth;
    public Animator animator;
    public Slider healthSlider;
    public GameObject healthBar;
    public GameObject DeathPanel;
    public bool bDeath = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = 1;
            healthSlider.value = 1f;
        }
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            healthSlider.value = healthPercent;
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        maxHealth += amount;
    }

    void Die()
    {
        bDeath = true;
        animator.SetBool("Death", true);
        healthBar.SetActive(false);
        DeathPanel.SetActive(true);
    }
}