using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public int dropSlotIndex;   // ref

    public CanvasGroup canvasGroup;
    public Vector3 slotPos;  // store initial position of slot
    RectTransform rt;

    public Item slotItemHeld;
    public Item draggableItemRef;   // reference to the item that was dragged

    // true if mouse is over this slot, ignores anything on top
    public bool IsHoveringOnSlot => RectTransformUtility.RectangleContainsScreenPoint(rt, Mouse.current.position.ReadValue(), null);

    // call event when counter ui is closed
    void OnEnable() { PickupCounterUI.OnCounterClosed += ResetState; }
    void OnDisable() { PickupCounterUI.OnCounterClosed -= ResetState; }

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (IsHoveringOnSlot)
            Debug.Log("Hovering over drop slot :)");
    }

    void Start()
    {
        // get position of current slot
        slotPos = transform.position;
    }

    // --- hotbar slot to drop icon --- 
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggable = eventData.pointerDrag.GetComponent<DraggableItem>();

        // check if dropped from hotbar
        // if (draggable.currentState == ItemCurrentState.InHotbar)
        //{
            if (draggable == null)
                return;

            draggableItemRef = draggable.GetCurrentHotbarSlotItem();
            slotItemHeld = draggableItemRef; // get dragged item into slot

            // draggable item realize it's currently in slot
            draggable.currentDropSlot = this;
            draggable.currentState = ItemCurrentState.InDropSlot;
            draggable.SetCurrentHotbarSlotItemNull();

            Debug.Log(HotbarManager.Instance.GetCurrentItemName(slotItemHeld));
        // }

        /*/ swap drop slots
        else
        {

        } */
    }


    // reset state maybe have check if abrupt
    public void ResetState()
    {
        // reset values
        slotItemHeld = null;
        draggableItemRef = null;
    }


    // check current drop slot item for debugging
    public void OnUserClick()
    {
        if (slotItemHeld != null)
            Debug.Log("Slot " + gameObject.name + " item: " + HotbarManager.Instance.GetCurrentItemName(slotItemHeld));
        else
            Debug.Log("Slot empty.");
    }
}
