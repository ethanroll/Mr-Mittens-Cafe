using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CanvasGroup canvasGroup;
    public int slotIndex;  // store what slot at
    public Item currentItem;

    // call event when counter ui is closed
    void OnEnable() { CounterUI.OnCounterClosed += ResetState; }
    void OnDisable() { CounterUI.OnCounterClosed -= ResetState; }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("current slot index: " + slotIndex);
        canvasGroup.blocksRaycasts = false; // lets the slot "see" what's dropped on it
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }


    // get current item at slot
    public Item GetCurrentSlotItem()
    {
        currentItem = HotbarManager.Instance.hotbar[slotIndex];
        return currentItem;
    }


    // set item at slot
    public void SetCurrentSlotItemNull()
    {
        HotbarManager.Instance.hotbar[slotIndex] = null;
        currentItem = null;
    }


    // reset state maybe have check if abrupt
}
