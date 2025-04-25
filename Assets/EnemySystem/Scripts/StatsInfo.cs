using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsInfo : MonoBehaviour
{
    public PlayerController player;
    public PlayerHealth playerHealth;
    
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI fireArrowText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critMultiplierText;
    
    
    
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        int currentDamage = player.baseDamage + player.level * 2 + Mathf.RoundToInt(player.bonusDamage);
        int currentHealth = playerHealth.currentHealth + player.level * 2 + Mathf.RoundToInt(player.bonusHealth);
        int currentfireArrow = player.baseDamage + player.bonusFireDamage + player.level * 2 + Mathf.RoundToInt(player.bonusDamage);
        float currentcritChance = player.critChance * 100f; // Преобразуем в проценты
        float currentcritMultiplier = player.critMultiplier;

        damageText.text = $"{currentDamage}";
        fireRateText.text = $"{player.fireRate:0.00}";
        healthText.text = $"{currentHealth}";
        fireArrowText.text = $"{currentfireArrow}";
        critChanceText.text = $"{currentcritChance:0.0}%"; // Например, 10.0%
        critMultiplierText.text = $"{currentcritMultiplier:0.0}x"; // Можно и тут добавить формат
    }
}
