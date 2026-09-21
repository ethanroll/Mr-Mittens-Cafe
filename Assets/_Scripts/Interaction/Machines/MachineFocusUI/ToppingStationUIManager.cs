using UnityEngine;
using System.Collections.Generic;
using System.Collections.Generic;

// sprite correspond to drink topping
[System.Serializable]
public struct ToppingSprite
{
    public DrinkTopping topping;
    public Sprite sprite;
}


public class ToppingStationUIManager : MonoBehaviour
{
    public static ToppingStationUIManager Instance;

    public List<ToppingSprite> toppingSpritesList;
    private Dictionary<DrinkTopping, Sprite> toppingSprites;
    
    void Awake()
    {
        Instance = this;

        toppingSprites = new Dictionary<DrinkTopping, Sprite>();

        // assign sprite for each topping
        foreach (ToppingSprite entry in toppingSpritesList)
            toppingSprites[entry.topping] = entry.sprite;
    }

    public Sprite GetSprite(DrinkTopping topping) => toppingSprites[topping];
}
