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

    private int activeSlot = 0;
    private int hotbarLength = 0;
    private Item currentHotbarSlot = null;

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
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)        // update current slot
            {
                for (int j = 0; j < hotbarKeys.Length; j++)
                {
                    hotbarPanel.transform.GetChild(j).GetComponent<Image>().color = Color.white; // reset all icons to unhighlight
                }
                activeSlot = i;
                hotbarPanel.transform.GetChild(i).GetComponent<Image>().color = Color.yellow; // add highlighted slot

                UserCurrentHotbarSlot();        // updates currentHotbarSlot to match the new activeSlot

                if (currentHotbarSlot != null)
                    printCurrentSlot(currentHotbarSlot);
            }
        }
    }

    // returns the current hotbar slot
    public Item UserCurrentHotbarSlot()
    {
        currentHotbarSlot = hotbar[activeSlot];
        return currentHotbarSlot;
    }

    // print current hotbar slot
    public void printCurrentSlot(Item item)
    {
        if (item == null)
        {
            Debug.Log("current slot is: empty");
            return;
        }

        string output = item.itemName; // start of building string of list

        if (item is Drink drink)
        {
            if (drink.cupSize != null) output += ", " + drink.cupSize;
            if (drink.drinkType != null) output += ", " + drink.drinkType;
            if (drink.temperature != null) output += ", " + drink.temperature;
            if (drink.numEspressoShots != null) output += ", " + drink.numEspressoShots;
            if (drink.iceLevel != null) output += ", " + drink.iceLevel;
            if (drink.milkType != null) output += ", " + drink.milkType;
        }

        if (item is Food food)
        {
            if (food.pastryType != null) output += ", " + food.pastryType;
            if (food.flavor != null) output += ", " + food.flavor;
            if (food.savoryType != null) output += ", " + food.savoryType;
        }
        Debug.Log("current slot is: " + output);
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
        hotbar[activeSlot] = null;
        currentHotbarSlot = null;

        Image icon = hotbarPanel.transform.GetChild(activeSlot).GetChild(0).GetComponent<Image>();
        icon.sprite = null;
        icon.enabled = false; // hides it so an empty slot doesn't show a blank white box
        ToastManager.Instance.DisplayMessage("removed item from hotbar");
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
