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

    // public RectTransform hotbarPanel;

    public CanvasGroup canvasGroup;
    public DropSlot currentDropSlot; // store ref to drop slot
    // public DropSlot swappingDropSlot;

    private Vector3 initialPos;  // store initial position of icon
    public int slotIndex;  // store what slot at

    public Item currentItem;

    private bool CanDrag => PickupCounter.Instance.canDragIcons;

    private bool handled;  // store if the item has been handled by a drop slot

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

        handled = false;

        if (currentState == ItemCurrentState.InHotbar)
        {
            // intial Pos = hotbar slot
            initialPos = transform.position;
            Debug.Log("current slot index: " + slotIndex);
        }

        canvasGroup.blocksRaycasts = false; // lets the slot "see" what's dropped on it
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag)
            return;

        transform.position = eventData.position; // follow the mouse          
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /* 1. in hotbar and drop on empty drop slot---
         * 2. in hotbar and drop on occupied drop slot (swap)
         * 3. in hotbar and drop on empty space---
         * 
         * 4. in drop slot and drop on empty drop slot---
         * 5. in drop slot and drop on occupied slow (swap)
         * 6. in drop slot and drop on hotbar
         * 7. in drop slot and drop on empty space--
         * 
         * ondrop -> onenddrag
         * 
         */


        // nothing handeled drop, go back to previous position
        if (!handled)
        {
            transform.position = (currentState == ItemCurrentState.InDropSlot) ? currentDropSlot.transform.position : initialPos;
        }

        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // something was dropped on this icon: if I'm sitting in a slot, treat it as a drop on that slot
        if (currentState == ItemCurrentState.InDropSlot && currentDropSlot != null)
            currentDropSlot.OnDrop(eventData);
    }



    // replaces the old OnDrop
    public void ReturnToHotbar()
    {
        if (currentState != ItemCurrentState.InDropSlot) return;

        transform.position = initialPos;
        currentState = ItemCurrentState.InHotbar;

        currentDropSlot.ResetState();
        HotbarManager.Instance.hotbar[slotIndex] = currentItem;
        currentDropSlot = null;

        handled = true;
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


    // move to drop slot position
    public void MoveToSlot(DropSlot slot)
    {
        // drop slot to another drop slot
        if (currentState == ItemCurrentState.InDropSlot)
        {
            if (currentDropSlot.heldIcon == this)
                currentDropSlot.ResetState();   // reset values of old drop slot
        }
        // leaving hotbar
        else
        {
            currentItem = HotbarManager.Instance.hotbar[slotIndex];
            SetCurrentHotbarSlotItemNull();  // set hotbar slot to null
        }

        // update new slot for item
        slot.slotItemHeld = currentItem;
        slot.heldIcon = this;
        currentDropSlot = slot;

        currentState = ItemCurrentState.InDropSlot;
        transform.position = slot.transform.position;

        handled = true;
    }


    // return item to hotbar
    public void ReturnToHotar()
    {
        if (currentState != ItemCurrentState.InDropSlot)
            return;

        currentDropSlot.ResetState();
        HotbarManager.Instance.hotbar[slotIndex] = currentItem;
        currentDropSlot = null;

        currentState = ItemCurrentState.InHotbar;
        transform.position = initialPos;

        handled = true;
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
