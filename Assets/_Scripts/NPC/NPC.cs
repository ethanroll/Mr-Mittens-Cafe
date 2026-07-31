using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    private NPC_Movement movement;  // reference to npc movement

    //private Drink drink;    // random drink
    public bool orderGiven = false;
    private Drink drink;
    private string orderName;

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

                StartCoroutine(OrderDialogue());
                OrderManager.Instance.GenerateRandomOrder(drink);
            }

            else if(orderGiven)
            {
                Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot

                if (CheckOrder(drink, currentItem))
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
        PlayerMovement.Instance.canMove = false;

        ToastManager.Instance.DisplayInteraction("Hi! I would like to order a..");
        yield return new WaitForSeconds(3f);

        ToastManager.Instance.DisplayInteraction(HotbarManager.Instance.GetCurrentItemName(drink));
        yield return new WaitForSeconds(5f);

        PlayerMovement.Instance.canMove = true;
        movement.OrderGiven(); // NPC walks to next counter
    }
}


