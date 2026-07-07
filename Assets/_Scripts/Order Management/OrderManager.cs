
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    private List<Order> orders = new List<Order>();    // store all orders

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
        //drink.DrinkType = GetRandomEnumValue<DrinkType>();
        drink.cupSize = GetRandomEnumValue<CupSize>();
        drink.temperature = GetRandomEnumValue<Temperature>();
        drink.numEspressoShots = UnityEngine.Random.Range(0, 4); // Random number of espresso shots between 0 and 3

        if (drink.temperature == Temperature.Iced)
        {
            drink.iceLevel = GetRandomEnumValue<IceLevel>();
        }

        drink.milkType = GetRandomEnumValue<MilkType>();

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
