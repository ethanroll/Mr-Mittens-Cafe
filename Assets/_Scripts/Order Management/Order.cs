using UnityEngine;

public class Order
{
    public Item requestedItem;  // drink or food items requested by the NPC
    public float waitTime;  // time the NPC is willing to wait for the order
    public bool isCompleted;  // whether the order has been completed
    public bool isCorrectOrder;  // whether the order was correct or not
}
