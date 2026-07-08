using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance;

    private Item[] hotbar = new Item[10]; // keys 0-10
    Key[] hotbarKeys = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5, Key.Digit6,
                             Key.Digit7, Key.Digit8, Key.Digit9, Key.Digit0 }; // store keypressed to corresponding hotbar slot

    public GameObject hotbarPanel;
    public GameObject slotIcon;
    [SerializeField] private GameObject hotbarSlotPrefab;

    private int activeSlot = -1;
    private Item currentHotbarSlot = null;
    public bool pressedOnce = false; // store if a hotbarkey was pressed at least once
    public bool drinkIsBusy = false; // store value for if drink is in a process
    private bool canPressAgain = false;
    public bool hasSlot = false; // store if activeSlot > -1



    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject slot = Instantiate(hotbarSlotPrefab, hotbarPanel.transform);  // instantiate 10 slots at start
            Image icon = slot.transform.GetChild(0).GetComponent<Image>();
            icon.enabled = false; // hide the empty icon so it doesn't block the highlight color
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hotbarKeys.Length; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame && !drinkIsBusy)        // update current slot, can't switch if drink is busy
            {
                pressedOnce = true;

                for (int j = 0; j < hotbarKeys.Length; j++)
                {
                    hotbarPanel.transform.GetChild(j).GetComponent<Image>().color = Color.white; // reset all icons to unhighlight
                }

                // if key is prressed again, deselect any items
                if (i == activeSlot && canPressAgain)
                {
                    hotbarPanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                    activeSlot = -1;
                    canPressAgain = false;
                    hasSlot = false;
                    currentHotbarSlot = null;
                }
                else
                {
                    canPressAgain = true;
                    activeSlot = i;

                    // check if input corresponds to a hotbar slot
                    if (activeSlot >= 0)
                        hasSlot = true;
                    else
                        hasSlot = false;

                    hotbarPanel.transform.GetChild(i).GetComponent<Image>().color = Color.yellow; // add highlighted slot

                    if(hasSlot)
                        UserCurrentHotbarSlot();        // updates currentHotbarSlot to match the new activeSlot
                }

                if (currentHotbarSlot != null)
                    GetCurrentItemName(currentHotbarSlot);
                    //printCurrentSlot(currentHotbarSlot);
            }
            else if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame && drinkIsBusy)
            {
                ToastManager.Instance.DisplayEquippedItem("Cannot switch items at the moment.");
            }
        }
    }


    // returns the current hotbar slot
    public Item UserCurrentHotbarSlot()
    {
        if(!hasSlot)
            return null;

        currentHotbarSlot = hotbar[activeSlot];
        return currentHotbarSlot;
    }


    // print current item name
    public string GetCurrentItemName(Item item)
    {
        string output = item.itemName;

        if (item is Drink drink)
        {
            if (drink.cupSize == null)
                return "Empty slot";
            if (drink.temperature == Temperature.Iced)
                output += "Iced ";

            // Espresso
            if (drink.milkType == null && drink.numEspressoShots == 2 && !drink.hasWater)
            {
                drink.drinkType = DrinkType.Espresso;
                output += $"{drink.cupSize} {drink.drinkType}";
            }
            // Americano
            else if(drink.milkType == null && (drink.numEspressoShots == 1 || drink.numEspressoShots ==2) && drink.hasWater)
            {
                drink.drinkType = DrinkType.Americano;
                output += $"{drink.cupSize} {drink.drinkType}";
            }
            else
            {
                return DefaultItemPrint(drink);
            }

            
        }
        ToastManager.Instance.DisplayEquippedItem(output);
        return output;
    }



    // print if drink is not a predetermined drink
    private string DefaultItemPrint(Item item)
    {
        string output = item.itemName;

        if (item is Drink drink)
        {
            if (drink.temperature != null) output += drink.temperature + " ";
            if (drink.cupSize != null) output += drink.cupSize + " cup";
            if (drink.iceLevel != null) output += " with " + drink.iceLevel + " ice";
            if (drink.numEspressoShots != 0) output += " with " + drink.numEspressoShots + " espresso shots";
            if (drink.milkType != null) output += " with " + drink.milkType + " milk";
            if (drink.hasWater) output += " with water";
        }
        ToastManager.Instance.DisplayEquippedItem(output);
        return output;     
    }


    // add Item to hotbar
    public void AddToHotbar(Item item)
    {       
        for(int i = 0; i < hotbar.Length; i++)  // check if array is full
        {
            if(hotbar[i] == null)               // place item in array if the index is null
            {
                hotbar[i] = item;

                Debug.Log("add to hotbar method called");
                Image icon = hotbarPanel.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                icon.sprite = item.icon;
                icon.enabled = true;
                break;
            }
        }
    }


    // remove Item from hotbar
    public void RemoveFromHotbar()
    {
        if (hasSlot)
        {
            hotbar[activeSlot] = null;
            currentHotbarSlot = null;

            Image icon = hotbarPanel.transform.GetChild(activeSlot).GetChild(0).GetComponent<Image>();
            icon.sprite = null;
            icon.enabled = false; // hides it so an empty slot doesn't show a blank white box
            ToastManager.Instance.DisplayInteraction("removed item from hotbar");
        }
    }


    // return value of isArrayFull
    public bool IsArrayFull()
    {
        foreach (var slot in hotbar)
        {
            if (slot == null) return false;
        }
        return true;
    }
}
