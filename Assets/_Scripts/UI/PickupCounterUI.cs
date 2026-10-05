using UnityEngine;
using UnityEngine.EventSystems;

public class PickupCounterUI : MonoBehaviour, ICancellable
{
    [SerializeField] private GameObject PickupCounterUIParent;
    [SerializeField] private GameObject DragAndDropArea;
    [SerializeField] private GameObject FinishButton;

    public void Cancel()
    {
        // reset values if userr cancel order before completing
        if (!PickupCounter.Instance.isOrderComplete)
        {
            // fire event when close
            OnCounterClosed?.Invoke();
        }


        // set ui inactive
        PickupCounterUIParent.SetActive(false);
        CancelManager.Instance.cancelButton.gameObject.SetActive(false);
    }
}