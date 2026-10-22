using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public int dropSlotIndex;   // ref

    public CanvasGroup canvasGroup;
    RectTransform rt;

    public Item slotItemHeld;   // ref to item held in slot
    public DraggableItem heldIcon;  // ref to draggable item icon held in slot


    // call event when counter ui is closed
    void OnEnable() { PickupCounter.OnCounterClosed += ResetState; }
    void OnDisable() { PickupCounter.OnCounterClosed -= ResetState; }

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }


    // drop onto drop slot
    public void OnDrop(PointerEventData eventData)
    {
        // item = draggable item dropped onto slot
        var item = eventData.pointerDrag != null? eventData.pointerDrag.GetComponent<DraggableItem>(): null;

        if (item == null) return;  // occupied = ignore 

        // drop item 1 and slot is empty
        if (slotItemHeld == null)
        {
            item.MoveToSlot(this);
        }

        // icon currently in this slot
        DraggableItem otherItem = heldIcon;
        if (otherItem == item) return;

        if (item.currentState == ItemCurrentState.InHotbar)
        {
            // kick the old icon back to the hotbar, then move in
            otherItem.ReturnToHotbar();
            item.MoveToSlot(this);
        }
        else
        {
            // other goes to item's old slot, item goes here
            DropSlot oldSlot = item.currentDropSlot;
            otherItem.MoveToSlot(oldSlot);
            item.MoveToSlot(this);
        }
    }


    // reset state maybe have check if abrupt
    public void ResetState()
    {
        // reset values
        slotItemHeld = null;
        heldIcon = null;
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
