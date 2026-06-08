using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance;
    public GameObject hotbarPanel;
    public GameObject slotIcon;
    [SerializeField] private GameObject hotbarSlotPrefab;
    public Item currentHotbarSlot = null;

    private Item[] hotbar = new Item[10]; // keys 0-10
    Key[] hotbarKeys = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5, Key.Digit6,
                             Key.Digit7, Key.Digit8, Key.Digit9, Key.Digit0 }; // store keypressed to corresponding hotbar slot
    int activeSlot = 0;
    

    private bool isArrayFull = false;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            Instantiate(hotbarSlotPrefab, hotbarPanel.transform);   // instantiate 10 slots at start
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
                UserCurrentHotbarSlot();
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
    public bool AddToHotbar(Item item)
    {
        for(int i = 0; i < hotbar.Length; i++)  // check if array is full
        {
            if(hotbar[i] == null)               // place item in array if the index is null
            {
                hotbar[i] = item;

                hotbarPanel.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = item.icon; // get icon of individual item & place on top of icon
                return isArrayFull;
            }
        }
        isArrayFull = true;
        return isArrayFull;
    }
}
