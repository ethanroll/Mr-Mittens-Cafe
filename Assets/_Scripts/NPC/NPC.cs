using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC : MonoBehaviour, IInteractable
{
    private NPC_Movement movement;  // reference to npc movement

    public int NPC_Number;  // store what number the npc is
    private Drink drink;
    private Food food;
    public bool orderGiven = false;
    
    public List<Item> requestedItems = new List<Item>();
    public List<bool> requestedItemsGiven = new List<bool>(); // bool val corresponding to if item was given

    void Awake()
    {
        movement = GetComponent<NPC_Movement>(); // get THIS npc's own movement script

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
                StartCoroutine(SayOrder());
            }
        }
    }

    private bool CheckOrder(Item currentOrder)
    {
        for (int i = 0; i < requestedItems.Count; i++)
        {
            Item requestedItem = requestedItems[i];
            bool isCorrectItem = false;

            if (requestedItem is Drink drinkOrder && currentOrder is Drink currentDrink && !requestedItemsGiven[i])
            {
                if (drinkOrder.cupSize == currentDrink.cupSize
                && drinkOrder.temperature == currentDrink.temperature
                && drinkOrder.milkType == currentDrink.milkType
                && drinkOrder.iceLevel == currentDrink.iceLevel
                && drinkOrder.hasWater == currentDrink.hasWater)
                {
                    isCorrectItem = true;
                    requestedItemsGiven[i] = isCorrectItem;
                    HotbarManager.Instance.RemoveFromHotbar();
                }
            }

            if (requestedItem is Food foodOrder && currentOrder is Food currentFood && !requestedItemsGiven[i])
            {
                if (foodOrder.pastryType == currentFood.pastryType) {
                    isCorrectItem = true;
                    requestedItemsGiven[i] = isCorrectItem;
                    HotbarManager.Instance.RemoveFromHotbar();
                }                 
                // add savory later
            }
        }

        // check if all incdices are true for requestedItemsGiven list
        bool allTrue = requestedItemsGiven.All(x => x);
        return allTrue;
    }
    

    private IEnumerator OrderDialogue()
    {
        PlayerMovement.Instance.canMove = false;

        ToastManager.Instance.DisplayInteraction("Hi! I would like to order..");
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(SayOrder()); // wait to finish

        PlayerMovement.Instance.canMove = true;
        movement.OrderGiven(); // NPC walks to next counter
    }

    private IEnumerator SayOrder()
    {
        if (orderGiven)
        {
            ToastManager.Instance.DisplayInteraction("My order is..");
            yield return new WaitForSeconds(2f);
        }

        for (int i = 0; i < requestedItems.Count; i++)
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
    }

    // check if NPC will order food
    public bool WillOrderFood()
    {
        if (OrderManager.Instance.foodSchedule[NPC_Number] == true)
            return true;
        return false;
    }
}


