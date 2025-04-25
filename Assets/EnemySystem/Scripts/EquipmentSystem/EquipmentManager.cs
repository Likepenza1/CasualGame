using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    [System.Serializable]
    public class SlotVisual
    {
        public EquipmentSlot slot;
        public SpriteRenderer renderer;
    }

    public List<SlotVisual> visuals;
    private Dictionary<EquipmentSlot, EquipmentItem> equippedItems = new();

    void Awake()
    {
        Instance = this;
    }

    public void Equip(EquipmentItem item)
    {
        equippedItems[item.slot] = item;
        UpdateVisual(item);
    }

    void UpdateVisual(EquipmentItem item)
    {
        var visual = visuals.Find(v => v.slot == item.slot);
        if (visual != null && visual.renderer != null)
            visual.renderer.sprite = item.sprite;
    }

    public EquipmentItem GetEquipped(EquipmentSlot slot)
    {
        equippedItems.TryGetValue(slot, out var item);
        return item;
    }
}