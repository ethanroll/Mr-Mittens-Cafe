using UnityEngine;

public class DropSlotReader : MonoBehaviour
{
    public static DropSlotReader Instance;

    [SerializeField] private DropSlot[] dropSlots = new DropSlot[3];

    void Awake()
    {
        Instance = this;
    } 

    public Item[] GetItemsInDropSlots()
    {
        Item[] items = new Item[dropSlots.Length];

        for (int i = 0; i < dropSlots.Length; i++)
        {
            items[i] = dropSlots[i] == null
                ? null
                : dropSlots[i].slotItemHeld;
        }

        return items;
    }

    public void LogItemsInDropSlots()
    {
        Item[] items = GetItemsInDropSlots();

        for (int i = 0; i < items.Length; i++)
        {
            string itemName = items[i] == null
                ? "Empty"
                : HotbarManager.Instance.GetCurrentItemName(items[i]);

            Debug.Log($"Drop slot {i + 1}: {itemName}");
        }
    }
}