using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    private NPC_Movement movement;  // reference to npc movement
    //private Order order;
    public bool orderGiven = false;
    private Drink drink;
    private Food food;
    public int NPC_Number;  // store what number the npc is

    public List<Item> requestedItems = new List<Item>();

    void Awake()
    {
        movement = GetComponent<NPC_Movement>(); // get THIS npc's own movement script
        //order = GetComponent<Order>(); // get THIS npc's own order script

        // Ensure SpriteSorter component exists on the NPC so it renders above counters
        if (GetComponent<SpriteSorter>() == null)
        {
            gameObject.AddComponent<SpriteSorter>();
        }
    }

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }


    // CHANGE SO CANT INTERACT AGAIN WHILE ALREADY INTERACTING
    public void Interact()
    {
            if (NPC_PickupManager.Instance.CheckTablesFull() && !orderGiven)
            {
                ToastManager.Instance.DisplayInteraction("The tables are full... I should wait til they clear up.");
            }

            else if (!NPC_PickupManager.Instance.CheckTablesFull() && !orderGiven)
            {
                drink = new Drink();

                // if have food
                food = new Food();

                StartCoroutine(OrderDialogue());
                OrderManager.Instance.GenerateRandomOrder(this, drink, food);
            }

            else if(orderGiven)
            {
                Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot

                if (CheckOrder(currentItem))
                {
                    ToastManager.Instance.DisplayInteraction("Thank you!");
                    movement.OrderReceived();  // NPC leaves
                }
                else
                {
                    HotbarManager.Instance.GetCurrentItemName(drink);
                    ToastManager.Instance.DisplayInteraction("Try again.");
                }
            }
    }

    private bool CheckOrder(Item currentOrder)
    {
        for (int i = 0; i < requestedItems.Count; i++)
        {
            Item requestedItem = requestedItems[i];

            if (requestedItem is Drink drinkOrder && currentOrder is Drink currentDrink)
            {
                return drinkOrder.cupSize == currentDrink.cupSize
                && drinkOrder.temperature == currentDrink.temperature
                && drinkOrder.milkType == currentDrink.milkType
                && drinkOrder.iceLevel == currentDrink.iceLevel
                && drinkOrder.hasWater == currentDrink.hasWater;
            }

            if (requestedItem is Food foodOrder && currentOrder is Food currentFood)
            {
                return foodOrder.pastryType == currentFood.pastryType;
            }
            //order.drinkType == current.DrinkType
        }
        return false;
    }
    

    private IEnumerator OrderDialogue()
    {
        PlayerMovement.Instance.canMove = false;

        ToastManager.Instance.DisplayInteraction("Hi! I would like to order..");
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < requestedItems.Count; i++)
        {
            Item currentItem = requestedItems[i];
            if (currentItem is Drink drink)
            {
                ToastManager.Instance.DisplayInteraction(HotbarManager.Instance.GetCurrentItemName(drink));
                yield return new WaitForSeconds(3f);
            }
            if (currentItem is Food food)
            {
                ToastManager.Instance.DisplayInteraction(HotbarManager.Instance.GetCurrentItemName(food));
                yield return new WaitForSeconds(3f);
            }
        }

        PlayerMovement.Instance.canMove = true;
        movement.OrderGiven(); // NPC walks to next counter
    }

    // check if NPC will order food
    public bool WillOrderFood()
    {
        if (OrderManager.Instance.foodSchedule[NPC_Number] == true)
            return true;
        return false;
    }
}


