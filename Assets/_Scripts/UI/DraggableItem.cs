using UnityEngine;
using UnityEngine.EventSystems;

// current state of draggable item
public enum ItemCurrentState
{
    InHotbar, InDropSlot
}


public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemCurrentState currentState;

    public CanvasGroup canvasGroup;
    public DropSlot currentDropSlot; // store ref to drop slot

    private Vector3 initialPos;  // store initial position of icon
    public int slotIndex;  // store what slot at

    private Item currentItem;

    private bool CanDrag => PickupCounter.Instance.canDragIcons;
    // public bool inDropSlot = false;

    // call event when counter ui is closed
    void OnEnable() { PickupCounterUI.OnCounterClosed += ResetState; }
    void OnDisable() { PickupCounterUI.OnCounterClosed -= ResetState; }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        currentState = ItemCurrentState.InHotbar;   // start in hotbar initially
    }


    // --- hotbar slot to drop icon --- 
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag)
            return;

        if (currentState == ItemCurrentState.InHotbar)
        {
            // intial Pos = hotbar slot
            initialPos = transform.position;
            Debug.Log("current slot index: " + slotIndex);
            canvasGroup.blocksRaycasts = false; // lets the slot "see" what's dropped on it
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag)
            return;

        transform.position = eventData.position; // follow the mouse          
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentState == ItemCurrentState.InHotbar)
            transform.position = initialPos; // reset pos
        else
            transform.position = currentDropSlot.slotPos;

        canvasGroup.blocksRaycasts = true;
    }


    // --- drop slot to hotbar icon ---
    public void OnDrop(PointerEventData eventData)
    {
        // get values back
        currentDropSlot.ResetState();
        HotbarManager.Instance.hotbar[slotIndex] = currentItem;

        currentState = ItemCurrentState.InHotbar;
        currentDropSlot = null;
    }


    // get current item at hotbar slot and store ref to original item
    public Item GetCurrentHotbarSlotItem()
    {
        currentItem = HotbarManager.Instance.hotbar[slotIndex];
        return currentItem;
    }


    // set null item at hotbar slot
    public void SetCurrentHotbarSlotItemNull()
    {
        HotbarManager.Instance.hotbar[slotIndex] = null;
    }


    // reset state maybe have check if abrupt
    public void ResetState()
    {
        // set hotbar slot to null if order completed
        if (PickupCounter.Instance.isOrderComplete)
        {
            SetCurrentHotbarSlotItemNull();
        }

        // else hotbar slot = original item value
        else
        {
            // reset pos if in drop slot
            if (currentState == ItemCurrentState.InDropSlot)
            {
                transform.position = initialPos;
                currentState = ItemCurrentState.InHotbar;
            }

            // reset value for next interaction
            HotbarManager.Instance.hotbar[slotIndex] = currentItem;
            currentItem = null; 
        }
    }
}
