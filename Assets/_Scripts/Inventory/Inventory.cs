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
            //Debug.Log("added item to actual list");
            items.Add(item);
            PrintList();
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

    // print entire current list
    public void PrintList()
    {
        foreach (Item item in items)
        {
            string output = item.itemName; // start of building string of list
            if (item is Drink drink)
            {
                if (drink.cupSize != null) output += ", " + drink.cupSize;
                if (drink.drinkType != null) output += ", " + drink.drinkType;
                if (drink.temperature != null) output += ", " + drink.temperature;
                if (drink.numEspressoShots != null) output += ", " + drink.numEspressoShots;
                if (drink.iceLevel != null) output += ", " + drink.iceLevel;
                if (drink.milkType != null) output += ", " + drink.milkType;
            }

            if(item is Food food)
            {
                if (food.pastryType != null) output += ", " + food.pastryType;
                if (food.flavor != null) output += ", " + food.flavor;
                if (food.savoryType != null) output += ", " + food.savoryType;
            }
            Debug.Log(output);
        }
    }
}
