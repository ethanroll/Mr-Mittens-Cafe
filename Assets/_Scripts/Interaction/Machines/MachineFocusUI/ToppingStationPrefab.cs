using UnityEngine;

public class ToppingStationPrefab : MonoBehaviour
{
    public DrinkTopping assignedTopping;

    public void OnToppingClick()
    {
        Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot();

        if (currentItem is Drink drink)// maybe fix later
            drink.drinkTopping = assignedTopping;
        // change sprite for drink
    }
}
