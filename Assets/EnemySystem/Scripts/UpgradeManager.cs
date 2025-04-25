using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("ссылки на игрока и здоровье")]
    public PlayerController player;
    public PlayerHealth playerHealth;

    [Header("Уровли улучшений")]
    public int damageLevel = 1;
    public int fireRateLevel = 1;
    public int healthLevel = 1;
    public int fireArrowLevel = 1;

    [Header("бонусы")]
    public float baseDamage = 10f;
    public float baseFireRate = 0.5f;
    
    [Header("стоимость")]
    public float upgradeCost = 50f;

    [Header("текст")]
    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI fireRateCostText;
    public TextMeshProUGUI healthCostText;
    public TextMeshProUGUI fireArrowCostText;

    [Header("кнопки")]
    public Button damageButton;
    public Button fireRateButton;
    public Button healthButton;
    public Button fireArrowButton;

    void Start()
    {
        UpdateUI();

        // Навешиваем обработчики кнопок
        damageButton.onClick.AddListener(UpgradeDamage);
        fireRateButton.onClick.AddListener(UpgradeFireRate);
        healthButton.onClick.AddListener(UpgradeHealth);
        fireArrowButton.onClick.AddListener(UpgradeFireArrow);
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        float damageCost = GetUpgradeCost(damageLevel);
        float fireRateCost = GetUpgradeCost(fireRateLevel);
        float healthCost = GetUpgradeCost(healthLevel);
        float fireArrowCost = GetUpgradeCost(fireRateLevel);
        
        int baseTotalDamage = player.baseDamage + player.level * 2 + Mathf.RoundToInt(player.bonusDamage);
        int fireBonus = player.bonusFireDamage;
        int totalDamage = baseTotalDamage + fireBonus;

        // Расчёт текущего урона по твоей формуле
        int currentfireArrow = player.baseDamage + player.bonusFireDamage + player.level * 2 + Mathf.RoundToInt(player.bonusDamage);
        int currentHealth = playerHealth.currentHealth + player.level * 2 + Mathf.RoundToInt(player.bonusHealth);
        
        damageCostText.text = $"Damage: {baseTotalDamage} + {fireBonus} = {totalDamage}  Price: {FormatGold(damageCost)}";
        fireRateCostText.text = $"Speed: {player.fireRate:0.00}  Price: {FormatGold(fireRateCost)}";
        healthCostText.text = $"Health: {currentHealth}  Price: {FormatGold(healthCost)}";
        fireArrowCostText.text = $"Fire damage: {currentfireArrow}  Price: {FormatGold(fireArrowCost)}";

        damageButton.interactable = player.gold >= damageCost;
        fireRateButton.interactable = player.gold >= fireRateCost;
        healthButton.interactable = player.gold >= healthCost;
        fireArrowButton.interactable = player.gold >= fireArrowCost;
    }

    public void UpgradeDamage()
    {
        float cost = GetUpgradeCost(damageLevel);
        if (player.gold >= cost)
        {
            player.gold -= cost;
            damageLevel++;
            player.bonusDamage += 2f;
        }
    }

    public void UpgradeFireRate()
    {
        float cost = GetUpgradeCost(fireRateLevel);
        if (player.gold >= cost)
        {
            player.gold -= cost;
            fireRateLevel++;
            player.fireRate += 0.1f;
        }
    }
    
    public void UpgradeHealth()
    {
        float cost = GetUpgradeCost(healthLevel);
        if (player.gold >= cost)
        {
            player.gold -= cost;
            healthLevel++;
            playerHealth.AddHealth(15);
        }
    }
    
    public void UpgradeFireArrow()
    {
        float cost = GetUpgradeCost(fireArrowLevel);
        if (player.gold >= cost)
        {
            player.gold -= cost;
            fireArrowLevel++;
            player.bonusFireDamage += 3; // Отдельный огненный урон
        }
    }

    private float GetUpgradeCost(int level)
    {
        return upgradeCost * Mathf.Pow(1.5f, level - 1);
    }

    string FormatGold(float goldAmount)
    {
        int goldInt = Mathf.FloorToInt(goldAmount);

        if (goldInt >= 1000000000)
            return $"{(goldInt / 1000000000f):0.##}B";
        else if (goldInt >= 1000000)
            return $"{(goldInt / 1000000f):0.##}M";
        else if (goldInt >= 1000)
            return $"{(goldInt / 1000f):0.##}K";
        else
            return goldInt.ToString();
    }
    
    public void ApplySaveData(SaveData data)
    {
        this.damageLevel = data.damageLevel > 0 ? data.damageLevel : 1;
        this.fireArrowLevel = data.fireArrowLevel > 0 ? data.fireArrowLevel : 1;
        this.fireRateLevel = data.fireRateLevel > 0 ? data.fireRateLevel : 1;
        this.healthLevel = data.healthLevel > 0 ? data.healthLevel : 1;
        this.upgradeCost = data.upgradeCost > 0 ? data.upgradeCost : 50f;
    }
}