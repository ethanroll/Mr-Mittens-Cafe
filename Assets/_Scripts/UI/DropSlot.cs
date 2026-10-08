using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public int dropSlotIndex;   // ref

    public CanvasGroup canvasGroup;
    public Vector3 slotPos;  // store initial position of slot
    RectTransform rt;

    public DraggableItem draggable;
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
        {
            // PickUpCounterUI.Instance.isHoveringOnSlot = true;
            PickupCounterUI.Instance.targetDropSlot = this;
            Debug.Log("Hovering over drop slot : " + PickupCounterUI.Instance.targetDropSlot.dropSlotIndex);
        }

        // only clear if this slot is the current target
        else if (PickupCounterUI.Instance.targetDropSlot == this)
        {
            PickupCounterUI.Instance.targetDropSlot = null;
            Debug.Log("Drop Slot = null");
            // PickUpCounterUI.Instance.isHoveringOnSlot = false;
        }
    }

    void Start()
    {
        // get position of current slot
        slotPos = transform.position;
    }

    // --- hotbar slot to drop icon --- 
    public void OnDrop(PointerEventData eventData)
    {
        draggable = eventData.pointerDrag.GetComponent<DraggableItem>();

        // 1. in hotbar and drop on empty drop slot
        if (draggable.currentState == ItemCurrentState.InHotbar && slotItemHeld == null)
        {
            if (draggable == null)
                return;

            draggableItemRef = draggable.GetCurrentHotbarSlotItem();
            slotItemHeld = draggableItemRef; // get dragged item into slot

            // draggable item realize it's currently in slot
            draggable.currentDropSlot = this;
            draggable.currentState = ItemCurrentState.InDropSlot;
            draggable.SetCurrentHotbarSlotItemNull();

            // draggable.transform.position = slotPos;

            Debug.Log(HotbarManager.Instance.GetCurrentItemName(slotItemHeld));
        }

        // 4. in drop slot and drop on empty drop slot
        if (draggable.currentState == ItemCurrentState.InDropSlot && draggable.currentDropSlot.slotItemHeld == null)
        {
            draggableItemRef = draggable.currentItem;
            slotItemHeld = draggableItemRef; // get dragged item into slot

            draggable.currentDropSlot = this;
            // draggable.currentState = ItemCurrentState.InDropSlot;

            draggable.transform.position = slotPos;
        }
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
