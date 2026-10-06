using UnityEngine;
using UnityEngine.EventSystems;

public class PickupCounterUI : MonoBehaviour
{
    public static PickupCounterUI Instance;

    public static event System.Action OnCounterClosed;


    void Awake()
    {
        Instance = this;
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