[System.Serializable]
public class SaveData
{
    public int level;
    public int nextlevel;
    public float xp;
    public float gold;
    public float bonusDamage;
    public float fireRate;
    public float bonusHealth;
    public float critChance;
    public float critMultiplier;
    public int selectedBackgroundID;

    // Новые поля для апгрейдов
    public int damageLevel;
    public int fireArrowLevel;
    public int fireRateLevel;
    public int healthLevel;
    public float upgradeCost;
}