using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<DraggableItem>() : null;
        
        if (item != null) 
            item.ReturnToHotbar();
    }
}

