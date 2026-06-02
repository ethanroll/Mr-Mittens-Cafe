using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private List<Item> items = new List<Item>();    // store items that player interacts with

    void Awake()
    {
        Instance = this;
    }

    // add items
    public void Add(Item item)
    {
        Item doesExist = items.Find(i => i.itemName == item.itemName);

        if (doesExist != null)
        {
            doesExist.quantity++;
            PrintList();
        }
        else
        {
            items.Add(item);
            PrintList();
        }
    }

    public void PrintList()
    {
        foreach (Item item in items)
        {
            Debug.Log(item.itemName + " " + item.type + " x" + item.quantity);
        }
    }

    // remove items
    //public void Remove()
    //{
        
   // }
}
