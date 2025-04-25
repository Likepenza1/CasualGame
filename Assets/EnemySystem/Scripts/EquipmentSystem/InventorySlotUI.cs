using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    private EquipmentItem item;

    public void Setup(EquipmentItem newItem)
    {
        item = newItem;
        icon.sprite = item.sprite;
    }

    public void OnClickEquip()
    {
        EquipmentManager.Instance.Equip(item);
        InventoryManager.Instance.RemoveItem(item);
    }
}