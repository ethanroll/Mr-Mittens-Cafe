using UnityEngine;

public class Item
{
    public string itemName;
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
    public int? numEspressoShots;
    public IceLevel? iceLevel;
    public MilkType? milkType;
}

public class Food: Item
{
    public PastryType? pastryType;
    public string? flavor;
    public SavoryType? savoryType;
}
