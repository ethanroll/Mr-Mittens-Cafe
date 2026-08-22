using UnityEngine;
using System.Collections.Generic;

public class Order
{
    public List<Item> requestedItems = new List<Item>();  // drink or food items requested by the NPC
    public List<bool> requestedItemsGiven = new List<bool>(); // bool val corresponding to if item was given   

    public float waitTime;  // time the NPC is willing to wait for the order
    public bool isCompleted;  // whether the order has been completed
    public bool isCorrectOrder;  // whether the order was correct or not
}
