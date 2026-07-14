
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    private List<Order> orders = new List<Order>();    // store all orders
    private int milkOrWater;    // store whether milk or water liquid will be chosen

    public void Awake()
    {
        Instance = this;
    }

    public Order GenerateRandomOrder(Drink drink)
    {
        Order generatedOrder = new Order();
        Item generatedItem = GenerateRandomDrink(drink);
        generatedOrder.requestedItem = generatedItem;
        return generatedOrder;
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
            drink.hasWater = true;

        Debug.Log(drink);
        HotbarManager.Instance.GetCurrentItemName(drink);
        return drink;
    }

    public void GenerateRandomFood()
    {
        Food food = new Food();

        // Randomly choose between pastry and savory
    }

    // generic method for getting random enum value for a drink/food order
    private T GetRandomEnumValue<T>() where T : System.Enum
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length));
    }


    //public bool CheckOrder()
    //{

    //}
}
