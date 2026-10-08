using UnityEngine;
using UnityEngine.EventSystems;

public class PickupCounterUI : MonoBehaviour
{
    // current state of PickupCounterUI
    public enum ItemCurrentState
    {
        GivingOrder, ChoosingNPC
    }

    public static PickupCounterUI Instance;

    public static event System.Action OnCounterClosed;


    // MAY CHANGE LATER
    public DropSlot targetDropSlot;   // store slot to swap with 
    // public bool isHoveringOnSlot = false;


    void Awake()
    {
        Instance = this;
    }


    public void SwapItems(DraggableItem currentDraggableItem)
    {
        // get original values of items to swap
        Item item1 = currentDraggableItem.currentItem;
        Item item2 = targetDropSlot.slotItemHeld;

        // swap values
        targetDropSlot.slotItemHeld = item1;
        currentDraggableItem.currentDropSlot.slotItemHeld = item2;

        // ref
        targetDropSlot.draggableItemRef = item1;
        currentDraggableItem.currentDropSlot.draggableItemRef = item2;

        // swap positions
        targetDropSlot.draggable.transform.position = currentDraggableItem.currentDropSlot.transform.position;
        currentDraggableItem.currentDropSlot.draggable.transform.position = targetDropSlot.transform.position;
    }


    public void PickupCounterUICancel()
    {
        // reset values if user cancel order before completing
        if (!PickupCounter.Instance.isOrderComplete)
        {
            // fire event when close
            OnCounterClosed?.Invoke();
        }
    }
}