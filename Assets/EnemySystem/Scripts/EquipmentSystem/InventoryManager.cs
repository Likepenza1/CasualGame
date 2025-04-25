using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<EquipmentItem> items = new();

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(EquipmentItem item)
    {
        items.Add(item);
        InventoryUI.Instance.Refresh();
    }

    public void RemoveItem(EquipmentItem item)
    {
        items.Remove(item);
        InventoryUI.Instance.Refresh();
    }
}