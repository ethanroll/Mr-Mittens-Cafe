using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC : MonoBehaviour, IInteractable
{
    private NPC_Movement movement;  // reference to npc movement

    [SerializeField] private Transform exitPoint;
    [SerializeField] private float speed = 5f;

    public int NPC_Number;  // store what number the npc is
    private float startWaitTime = 10f;    // how long npc will wait
    private float endWaitTime = 20f;

    public Order CurrentOrder { get; private set; }

    private bool givingOrder = false;
    private bool orderReceived = false;
    public bool startTimeExceeded = false;
    public bool endTimeExceeded = false;

    private Drink drink;
    private Food food;
    public bool orderGiven = false;



    void Awake()
    {
        movement = GetComponent<NPC_Movement>(); // get THIS npc's own movement script

        // Ensure SpriteSorter component exists on the NPC so it renders above counters
        if (GetComponent<SpriteSorter>() == null)
        {
            gameObject.AddComponent<SpriteSorter>();
        }
    }

    void Update()
    {
        if (!givingOrder && (movement.currentState == NPC_Movement.NPC_State.WalkToCounter || movement.currentState == NPC_Movement.NPC_State.WaitAtCounter || movement.currentState == NPC_Movement.NPC_State.InQueue))
        {
            if (startWaitTime > 0)
            {
                startWaitTime -= Time.deltaTime;

                if (startWaitTime < 0)
                    startWaitTime = 0;
            }
            else
            {
                startTimeExceeded = true;
            }
        }

        else if (!orderReceived && (movement.currentState == NPC_Movement.NPC_State.WaitForPickup))
        {
            if (endWaitTime > 0)
            {
                endWaitTime -= Time.deltaTime;

                if (endWaitTime < 0)
                    endWaitTime = 0;
            }
            else
            {
                endTimeExceeded = true;
            }
        }
    }

    public void AssignOrder(Order order)
    {
        CurrentOrder = order;
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
            // start dialogue when initiating order and generate order
            givingOrder = true;

            drink = new Drink();
            food = new Food();

            // store new order
            Order newOrder = OrderManager.Instance.GenerateRandomOrder(this, drink, food);
            AssignOrder(newOrder);

            StartCoroutine(OrderDialogue());
        }

        else if (orderGiven)
        {
            Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot

            if (CheckOrder(currentItem))
            {
                orderReceived = true;
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
        for (int i = 0; i < CurrentOrder.requestedItems.Count; i++)
        {
            Item requestedItem = CurrentOrder.requestedItems[i];
            bool isCorrectItem = false;

            if (requestedItem is Drink drinkOrder && currentOrder is Drink currentDrink && !CurrentOrder.requestedItemsGiven[i])
            {
                if (drinkOrder.cupSize == currentDrink.cupSize
                && drinkOrder.temperature == currentDrink.temperature
                && drinkOrder.milkType == currentDrink.milkType
                && drinkOrder.iceLevel == currentDrink.iceLevel
                && drinkOrder.waterFilled == currentDrink.waterFilled)
                {
                    isCorrectItem = true;
                    CurrentOrder.requestedItemsGiven[i] = isCorrectItem;
                    HotbarManager.Instance.RemoveFromHotbar();
                }
            }

            if (requestedItem is Food foodOrder && currentOrder is Food currentFood && !CurrentOrder.requestedItemsGiven[i])
            {
                if (foodOrder.pastryType == currentFood.pastryType)
                {
                    isCorrectItem = true;
                    CurrentOrder.requestedItemsGiven[i] = isCorrectItem;
                    HotbarManager.Instance.RemoveFromHotbar();
                }
                // add savory later
            }
        }

        // check if all incdices are true for requestedItemsGiven list
        bool allTrue = CurrentOrder.requestedItemsGiven.All(x => x);
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

        OrderTrackingUI.Instance.AddTicket(this);  // add to ticket for UI
    }

    private IEnumerator SayOrder()
    {
        if (orderGiven)
        {
            ToastManager.Instance.DisplayInteraction("My order is..");
            yield return new WaitForSeconds(2f);
        }

        for (int i = 0; i < CurrentOrder.requestedItems.Count; i++)
        {
            Item currentItem = CurrentOrder.requestedItems[i];
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

    /// <summary>
    /// Returns formatted order item descriptions and their delivery status for UI display.
    /// </summary>
    public List<string> GetFormattedOrderDetails()
    {
        List<string> details = new List<string>();

        for (int i = 0; i < CurrentOrder.requestedItems.Count; i++)
        {
            Item item = CurrentOrder.requestedItems[i];
            bool isDelivered = i < CurrentOrder.requestedItemsGiven.Count && CurrentOrder.requestedItemsGiven[i];

            string itemName = HotbarManager.Instance != null ? HotbarManager.Instance.GetCurrentItemName(item) : item.itemName;
            string statusTag = isDelivered ? "<color=#00FF00>[Delivered]</color>" : "<color=#FFA500>[Pending]</color>";

            details.Add($"- {itemName} {statusTag}");
        }

        return details;
    }
}