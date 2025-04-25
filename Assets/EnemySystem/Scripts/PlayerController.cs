using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("стрелы")]
    public GameObject bulletPrefab;
    public Transform spawnPos;
    public float bulletSpeed = 20f;
    public float maxDistance = 10f;
    public bool autoshot = false;
    private float nextFireTime;
    private GameObject currentTarget;
    
    [Header("урон, криты, бонусы")]
    public int baseDamage = 10; // базовый урон
    public float bonusDamage = 0; // бонус от апгрейдов
    public int bonusFireDamage = 0; // бонус от апгрейдов
    public float bonusHealth = 0; // бонус от апгрейдов
    public float fireRate = 0.5f;
    public float critChance = 0.1f; // 10%
    public float critMultiplier = 2f; // x2 урон
    
    [Header("аниматор")]
    public Animator animator;

    [Header("система уровней")]
    // XP и Level
    public int level = 1;
    private int nextLevel = 2;
    public float currentXP = 0;
    public float gold = 0;
    public Slider xpSlider;

    public float XPToNextLevel => 100f * Mathf.Pow(1.5f, level - 1);

    [Header("текст")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelNextText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI goldText;
    
    [Header("здоровье игрока")]
    public PlayerHealth playerHealth;
    

    void Start()
    {
        // Инициализация слайдера
        if (xpSlider != null)
        {
            xpSlider.minValue = 0;
            xpSlider.maxValue = 1;
            xpSlider.value = 0;
        }

        SaveData loadedData = SaveManager.Load();
        if (loadedData != null)
        {
            ApplySaveData(loadedData);

            BackgroundManager bgManager = FindObjectOfType<BackgroundManager>();
            if (bgManager != null)
            {
                bgManager.LoadBackgroundByID(loadedData.selectedBackgroundID);
            }
        }
    }

    void Update()
    {
        if (playerHealth.bDeath == false)
        {
            if (autoshot && Time.time >= nextFireTime)
            {
                currentTarget = FindClosestEnemyInRange(maxDistance);
                if (currentTarget != null)
                {
                    animator.SetTrigger("ShotBow");
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                currentTarget = FindClosestEnemyInRange(maxDistance);
                if (currentTarget != null)
                {
                    animator.SetTrigger("ShotBow");
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }

        UpdateUI();
    }

    public void BowSoot()
    {
        Debug.Log("BowSoot вызван");

        if (playerHealth.bDeath == false)
        {
            if (currentTarget != null)
            {
                Vector2 direction = (currentTarget.transform.position - spawnPos.position).normalized;

                GameObject bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                ProjectileMover proj = bullet.GetComponent<ProjectileMover>();
                if (proj != null)
                {
                    proj.SetOwner(this);
                }
            }
        }
    }

    public void GainXP(float amount)
    {
        currentXP += amount;
        while (currentXP >= XPToNextLevel)
        {
            currentXP -= XPToNextLevel;
            level++;
            nextLevel++;
            Debug.Log("Level Up! Новый уровень: " + level);
        }
        Debug.Log("Current Level: " + level);  // Проверка уровня после прокачки
    }

    public void AddGold(float amount)
    {
        if (level > 0)  // Убедитесь, что level > 0
        {
            gold += amount * level;
            Debug.Log($"Added {amount * level} gold. Total gold: {gold}");
        }
        else
        {
            Debug.LogWarning("Level is 0, cannot add gold.");
        }
    }

    GameObject FindClosestEnemyInRange(float maxRange)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Ai");
        GameObject closestEnemy = null;
        float closestDistance = maxRange;
        Vector3 playerPos = spawnPos.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, playerPos);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
    
    public int GetTotalDamage()
    {
        float multiplier = 1f + (level - 1) * 0.15f;
        float total = baseDamage * multiplier + bonusDamage + bonusFireDamage;

        bool isCrit = UnityEngine.Random.value < critChance;
        if (isCrit)
        {
            total *= critMultiplier;
        }

        return Mathf.CeilToInt(total);
    }
    
    string FormatGold(float goldAmount)
    {
        // Округляем до целого числа
        int goldInt = Mathf.FloorToInt(goldAmount);
        
        if (goldInt >= 1000000000) // Более 1 миллиарда
        {
            return $"{(goldInt / 1000000000f):0.##}B";
        }
        else if (goldInt >= 1000000) // Более 1 миллиона
        {
            return $"{(goldInt / 1000000f):0.##}M";
        }
        else if (goldInt >= 1000) // Более 1 тысячи
        {
            return $"{(goldInt / 1000f):0.##}K";
        }
        else // Менее 1000
        {
            return goldInt.ToString();
        }
    }

    void UpdateUI()
    {
        if (levelText != null) levelText.text = $"{level}";
        if (levelNextText != null) levelNextText.text = $"{nextLevel}";
        if (xpText != null) xpText.text = $"{currentXP}/{XPToNextLevel}";
        if (goldText != null) goldText.text = FormatGold(gold);
        
        if (xpSlider != null)
        {
            float progress = currentXP / XPToNextLevel;
            xpSlider.value = Mathf.Clamp01(progress);
        }
    }
    
    void OnApplicationQuit()
    {
        SaveData dataToSave = CreateSaveData();
        SaveManager.Save(dataToSave);
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData dataToSave = CreateSaveData();
            SaveManager.Save(dataToSave);
        }
    }
    
    public SaveData CreateSaveData()
    {
        UpgradeManager upgradeManager = FindObjectOfType<UpgradeManager>();

        return new SaveData
        {
            level = this.level,
            nextlevel = this.nextLevel,
            xp = this.currentXP,
            gold = this.gold,
            bonusDamage = this.bonusDamage,
            fireRate = this.fireRate,
            bonusHealth = this.bonusHealth,
            critChance = this.critChance,
            critMultiplier = this.critMultiplier,
            selectedBackgroundID = FindObjectOfType<BackgroundManager>()?.GetCurrentBGID() ?? 0,

            // Апгрейды
            damageLevel = upgradeManager != null ? upgradeManager.damageLevel : 1,
            fireRateLevel = upgradeManager != null ? upgradeManager.fireRateLevel : 1,
            healthLevel = upgradeManager != null ? upgradeManager.healthLevel : 1,
            upgradeCost = upgradeManager != null ? upgradeManager.upgradeCost : 50f
        };
    }
    
    public void ApplySaveData(SaveData data)
    {
        this.level = data.level > 0 ? data.level : 1;
        this.nextLevel = data.nextlevel > 0 ? data.nextlevel : 2;
        this.currentXP = data.xp;
        this.gold = data.gold;
        this.bonusDamage = data.bonusDamage;
        this.fireRate = data.fireRate > 0 ? data.fireRate : 0.5f;
        this.bonusHealth = data.bonusHealth;
        this.critChance = data.critChance > 0 ? data.critChance : 0.1f;
        this.critMultiplier = data.critMultiplier > 0 ? data.critMultiplier : 2f;

        BackgroundManager bgManager = FindObjectOfType<BackgroundManager>();
        if (bgManager != null)
        {
            bgManager.SetBGByID(data.selectedBackgroundID);
        }

        // Применяем апгрейды
        FindObjectOfType<UpgradeManager>()?.ApplySaveData(data);
    }
} 