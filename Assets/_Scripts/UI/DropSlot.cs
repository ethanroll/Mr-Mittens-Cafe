using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public CanvasGroup canvasGroup;
    public Vector3 slotPos;  // store initial position of slot

    public Item slotItemHeld;
    public Item draggableItemRef;   // reference to the item that was dragged

    // public bool inHotbarSlot = false;

    // call event when counter ui is closed
    void OnEnable() { PickupCounterUI.OnCounterClosed += ResetState; }
    void OnDisable() { PickupCounterUI.OnCounterClosed -= ResetState; }


    void Start()
    {
        // get position of current slot
        slotPos = transform.position;
    }

    // --- hotbar slot to drop icon --- 
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggable = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggable == null)
            return;

        draggableItemRef = draggable.GetCurrentHotbarSlotItem();
        slotItemHeld = draggableItemRef; // get dragged item into slot

        // draggable item realize it's currently in slot
        draggable.currentDropSlot = this;
        draggable.currentState = ItemCurrentState.InDropSlot;
        draggable.SetCurrentHotbarSlotItemNull();

        Debug.Log(HotbarManager.Instance.GetCurrentItemName(slotItemHeld));
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
