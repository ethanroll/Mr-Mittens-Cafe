using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private List<Item> items = new List<Item>();    // store items that player interacts with

    void Awake()
    {
        Instance = this;
    }


    // add items to list
    public void Add(Item item)
    {
        if (!HotbarManager.Instance.IsArrayFull()) // if false add item to list
        {
            items.Add(item);
        }
    }


    // remove currently selected item from inventory
    public void Remove()
    {
        if (HotbarManager.Instance.UserCurrentHotbarSlot() != null && HotbarManager.Instance.pressedOnce)
        {
            Item item = HotbarManager.Instance.UserCurrentHotbarSlot();
            items.Remove(item);
            Debug.Log("removed item from actual list");
            HotbarManager.Instance.RemoveFromHotbar();      // remove item from hotbar
        }
    }
}
