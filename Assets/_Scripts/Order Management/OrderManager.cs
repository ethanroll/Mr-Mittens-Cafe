using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public NPC npc;

    public static OrderManager Instance;
    private List<Order> orders = new List<Order>();    // store all orders
    public List<bool> foodSchedule = new List<bool>(); // store if NPC will order food
    public List<bool> orderGiven = new List<bool>();   // store if order was given

    private int milkOrWater;    // store whether milk or water liquid will be chosen

    private bool foodScheduleFinished = false;
    //public bool willOrderFood = false;
    private int numOrderingFood = 0;
    private int foodOrderQuota = 5;

    public void Awake()
    {
        // npc = GetComponent<NPC>();  // MIGHT NOT BE NEEDED ANYMORE
        Instance = this;
    }

    void Start()
    {
         PopulateFoodSchedule();
    }

    public Order GenerateRandomOrder(NPC npc, Drink drink, Food food)
    {
        Order npcOrder = new Order(); // store order

        // fill out NPC order details
        drink = GenerateRandomDrink(drink);
        npcOrder.requestedItems.Add(drink);

        // chance for a food item
        if (npc.WillOrderFood())
        {
            food = GenerateRandomFood(food);
            npcOrder.requestedItems.Add(food);
        }

        // populate requestedItemsGiven
        for (int i = 0; i < npcOrder.requestedItems.Count; i++)
        {
            npcOrder.requestedItemsGiven.Add(false); // all false to start i.e food not given yet
        }
        return npcOrder;
    }

    public Drink GenerateRandomDrink(Drink drink)
    {
        // chance for milk or water
        milkOrWater = UnityEngine.Random.Range(0, 2);

        //drink.DrinkType = GetRandomEnumValue<DrinkType>();
        drink.cupSize = GetRandomEnumValue<CupSize>();
        drink.temperature = GetRandomEnumValue<Temperature>();
        drink.numEspressoShots = UnityEngine.Random.Range(0, 4); // Random number of espresso shots between 0 and 3

        if (drink.temperature == Temperature.Iced)
        {
            drink.iceLevel = GetRandomEnumValue<IceLevel>();
        }

        if (milkOrWater == 0)
            drink.milkType = GetRandomEnumValue<MilkType>(); 
        else
            drink.waterFilled = true;

        HotbarManager.Instance.GetCurrentItemName(drink);
        return drink;
    }

    public Food GenerateRandomFood(Food food)
    {
        food = new Food();

        // CHANGE LATER
        food.pastryType = PastryType.ApplePie;
        HotbarManager.Instance.GetCurrentItemName(food);
        // Randomly choose between pastry and savory
        return food;
    }

    // method to shuffle vales, fisher yates method
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]); // swap
        }
    }

    // showcase which npcs will order food
    public void PopulateFoodSchedule()
    {
        // prefill with false so every index exists
        for (int i = 0; i < NPC_Manager.Instance.numNPC_Cap; i++)
            foodSchedule.Add(false);

        List<int> indices = Enumerable.Range(0, NPC_Manager.Instance.numNPC_Cap).ToList();
        Shuffle(indices); // fisher yates method

        foreach (int i in indices)
        {
            if (numOrderingFood >= foodOrderQuota)
                break;

            foodSchedule[i] = true;
            numOrderingFood++;
        }
    }

    // generic method for getting random enum value for a drink/food order
    private T GetRandomEnumValue<T>() where T : System.Enum
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    // get a list of all the active NPCs orders
    public void GetActiveNPCsWithOrders()
    {







        /*
        List<NPC> activeNPCs = new List<NPC>();

#if UNITY_2023_1_OR_NEWER
        NPC[] allNPCs = FindObjectsByType<NPC>(FindObjectsSortMode.None);
#else
        NPC[] allNPCs = FindObjectsOfType<NPC>();
#endif
        foreach (NPC npcItem in allNPCs)
        {
            if (npcItem != null && npcItem.orderGiven && npcItem.requestedItems != null && npcItem.requestedItems.Count > 0)
            {
                activeNPCs.Add(npcItem);
            }
        }

        activeNPCs.Sort((a, b) => a.NPC_Number.CompareTo(b.NPC_Number));
        return activeNPCs; */
    } 
}

