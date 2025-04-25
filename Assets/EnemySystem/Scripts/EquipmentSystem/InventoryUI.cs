using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public Transform contentParent;
    public GameObject slotPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var item in InventoryManager.Instance.items)
        {
            var slot = Instantiate(slotPrefab, contentParent).GetComponent<InventorySlotUI>();
            slot.Setup(item);
        }
    }
}