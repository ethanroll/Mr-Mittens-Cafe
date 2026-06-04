using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance;
    public GameObject hotbarPanel;
    public GameObject slotIcon;
    [SerializeField] private GameObject hotbarSlotPrefab;

    private Item[] hotbar = new Item[10]; // keys 0-10
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
        if
    }

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
