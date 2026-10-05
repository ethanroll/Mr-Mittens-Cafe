using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public Item slotItemHeld;
    public Item draggableItemRef;   // reference to the item that was dragged

    // call event when counter ui is closed
    void OnEnable() { CounterUI.OnCounterClosed += ResetState; }
    void OnDisable() { CounterUI.OnCounterClosed -= ResetState; }

    // let go of icon
    public void OnDrop(PointerEventData eventData)
    {
        draggableItemRef = eventData.pointerDrag.GetComponent<DraggableItem>().GetCurrentSlotItem();
        slotItemHeld = draggableItemRef; // get dragged item into slot

        eventData.pointerDrag.GetComponent<DraggableItem>().SetCurrentSlotItemNull();

        Debug.Log(HotbarManager.Instance.GetCurrentItemName(slotItemHeld));
       // Debug.Log(eventData.pointerDrag.name);
    }


    // assign item to slot
    private void AssignSlotItem()
    {
       // slotItemHeld = 
    }

    // reset state maybe have check if abrupt
}
