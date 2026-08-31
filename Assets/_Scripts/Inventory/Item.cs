using UnityEngine;

public class Item
{
    public string itemName; // ADD LATER FOR HOTBAR NAME 
    //public ItemType itemType;
    //public int quantity; // might not need qty
    public Sprite icon;
}

// '?' after value type makes it nullable
public class Drink: Item
{
    public CupSize? cupSize;
    public DrinkType? drinkType;
    public Temperature? temperature;
    public int numEspressoShots = 0;
    public IceLevel? iceLevel;
    public MilkType? milkType;

    public bool waterFilled;
    public float waterFillProgress = 0f;

    public bool milkAdded;
    public float milkFillProgress = 0f;
    public bool milkOverflow = false; // store if milk has overflowed out of the cup
}

public class Food: Item
{
    public PastryType? pastryType;
    //public string? flavor;
    public SavoryType? savoryType;
}
