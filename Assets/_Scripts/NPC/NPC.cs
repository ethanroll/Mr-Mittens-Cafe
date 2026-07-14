using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    //private Drink drink;    // random drink
    private bool orderGiven = false;
    private Drink drink;
    private string orderName;

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        if (!orderGiven)
        {
            drink = new Drink();

            StartCoroutine(OrderDialogue());
            OrderManager.Instance.GenerateRandomOrder(drink);
        }
        else
        {
            Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot

            if(CheckOrder(drink, currentItem))
            {
                ToastManager.Instance.DisplayInteraction("Thank you!");
                NPC_Movement.Instance.OrderReceived();  // NPC leaves
            }
            else
            {
                HotbarManager.Instance.GetCurrentItemName(drink);
                ToastManager.Instance.DisplayInteraction("Try again.");
            }
        }
    }

    private bool CheckOrder(Item order, Item current)
    {
        if (order is Drink drinkOrder && current is Drink currentDrink)
        {
            return drinkOrder.cupSize == currentDrink.cupSize
            && drinkOrder.temperature == currentDrink.temperature
            && drinkOrder.milkType == currentDrink.milkType
            && drinkOrder.iceLevel == currentDrink.iceLevel
            && drinkOrder.hasWater == currentDrink.hasWater;
        }
        //order.drinkType == current.DrinkType

        return false;
    }

    private IEnumerator OrderDialogue()
    {
        ToastManager.Instance.DisplayInteraction("Hi! I would like to order a..");
        yield return new WaitForSeconds(3f);
        ToastManager.Instance.DisplayInteraction(HotbarManager.Instance.GetCurrentItemName(drink));
        yield return new WaitForSeconds(5f);

        orderGiven = true;
        NPC_Movement.Instance.OrderGiven(); // NPC walks to next counter
    }
}


