using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/Item")]
public class EquipmentItem : ScriptableObject
{
    public string itemName;
    public EquipmentSlot slot;
    public Sprite sprite;
    public int bonusDamage;
    public int bonusHealth;
}