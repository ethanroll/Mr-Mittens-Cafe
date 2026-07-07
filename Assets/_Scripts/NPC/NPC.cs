using UnityEngine;
using UnityEngine.AI;

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
            OrderManager.Instance.GenerateRandomOrder(drink);
            orderGiven = true;
        }
        else
        {
            Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot

            if(CheckOrder(drink, currentItem))
            {
                Debug.Log("Order success!");
            }
            else
            {
                HotbarManager.Instance.GetCurrentItemName(drink);
                Debug.Log("Try again.");
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
            && drinkOrder.iceLevel == currentDrink.iceLevel;
        }
        //order.drinkType == current.DrinkType

        return false;
    }
    /*
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        agent.SetDestination(GameObject.FindGameObjectWithTag("Target").transform.position);
    }
    */
    // Order newOrder = new OrderManager.Instance.GenerateRandomOrder();
    // npc.CurrentOrder = newOrder;
}


