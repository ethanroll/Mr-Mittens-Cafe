using UnityEngine;

public class PointChecker : MonoBehaviour
{
    // int maxOrderPts = 300;

    /* Set values T or F
     * cup size - 30
     * temperature - 30
     * topping - 30
     * milk type/ water - 30
     * 
     * need tolerance level
     * ice level - 50
     * num espresso - 50
     * milk fill - 50
     * 
     * food - 30
     */

    public static PointChecker Instance;

    // - fixed scores: 60
    [SerializeField] private int cupScore = 30;
    [SerializeField] private int tempScore = 30;
    [SerializeField] private int toppingScore = 30;

    // -- liquid score: 30 (split milk/water so it's fair)
    [SerializeField] private int milkTypeScore = 13;
    [SerializeField] private int milkAmtScore = 17;
    [SerializeField] private int waterScore = 30;

    [SerializeField] private int iceScore = 50;
    [SerializeField] private int espressoScore = 50;

    [SerializeField] private int foodScore = 33;

    void Awake()
    {
        Instance = this;
    }

    // check if order is correct
    public int AddPoints(Item currentItem, Order CurrentOrder)
    {
        int score = 0;

        for (int i = 0; i < CurrentOrder.requestedItems.Count; i++)
        {
            Item requestedItem = CurrentOrder.requestedItems[i];
            // bool isCorrectItem = false;

            if (requestedItem is Drink drinkOrder && currentItem is Drink currentDrink && !CurrentOrder.requestedItemsGiven[i])
            {
                if (drinkOrder.cupSize == currentDrink.cupSize)
                {
                    score += cupScore;
                }
                if (drinkOrder.temperature == currentDrink.temperature)
                {
                    score += tempScore;
                }
                if (drinkOrder.milkType == currentDrink.milkType)
                {
                    score += milkTypeScore;
                }

                // change later to have set amt or somethign
                if (currentDrink.milkFillProgress >= 0)
                {
                    score += milkAmtScore;
                }

                if (drinkOrder.iceLevel == currentDrink.iceLevel)
                {
                    score += iceScore;
                }
                if (currentDrink.waterFillProgress >= 0)
                {
                    score += waterScore;
                }
                if (drinkOrder.numEspresso == currentDrink.numEspresso)
                {
                    score += espressoScore;
                }

                HotbarManager.Instance.RemoveFromHotbar();
            }

            if (requestedItem is Food foodOrder && currentItem is Food currentFood && !CurrentOrder.requestedItemsGiven[i])
            {
                if (foodOrder.pastryType == currentFood.pastryType)
                {
                    score += foodScore;
                    HotbarManager.Instance.RemoveFromHotbar();
                }
                // add savory later
            }
        }
        return score;
    }   
}
