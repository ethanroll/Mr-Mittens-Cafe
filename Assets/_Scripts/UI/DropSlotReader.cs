using UnityEngine;
using System.Collections.Generic;

public class DropSlotReader : MonoBehaviour
{
    public static DropSlotReader Instance;

    [System.NonSerialized] public List<DropSlot> dropSlots = new List<DropSlot>();

    void Awake()
    {
        Instance = this;
    }

    /*
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
    } */

    public void Register(DropSlot slot)
    {
        if (!dropSlots.Contains(slot)) dropSlots.Add(slot);
    }

    
    private bool ItemsMatch(Item a, Item b)
    {
        if (a is Drink da && b is Drink db)
        {
            return da.cupSize == db.cupSize
                && da.temperature == db.temperature
                && da.milkType == db.milkType
                && da.iceLevel == db.iceLevel
                && da.waterFilled == db.waterFilled;
        }

        if (a is Food fa && b is Food fb)
            return fa.pastryType == fb.pastryType;

        return false;
    }


    // compare items in drop slots with the npc's order items
    public bool CheckItems(NPC npc)
    {
        var remaining = new List<Item>(npc.CurrentOrder.requestedItems);

        foreach (var slot in dropSlots)
        {
            Item held = slot.slotItemHeld;
            if (held == null) continue;

            int match = remaining.FindIndex(r => ItemsMatch(held, r));
            if (match < 0) return false;   // something in a slot wasn't ordered
            remaining.RemoveAt(match);     // consume it so duplicates count correctly
        }

        return remaining.Count == 0;       // everything ordered was provided
    }
}