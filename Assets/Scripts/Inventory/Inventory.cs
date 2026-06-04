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

    // add items to list
    public void Add(Item item)
    {
        if (HotbarManager.Instance.AddToHotbar(item) == false) // if false add item to list
        {
            //Item doesExist = items.Find(i => i.itemName == item.itemName);

            //if (doesExist != null)
            //{
            //doesExist.quantity++;
            // PrintList();
            //}
            //else
            //{
            Debug.Log("added item to actual list");
                items.Add(item);
                PrintList();
            //}
        }
        else {
            Debug.Log("Can't add anymore items");
        }
    }

    // check if player has cup
    //public bool HasCup()
   // {

   // }

    // check what player currently has to combine


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

    // remove items
    //public void Remove()
    //{
        
   // }
}
