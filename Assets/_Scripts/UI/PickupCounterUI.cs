using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickupCounterUI : MonoBehaviour, INext_Previous
{
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject nextButton;


    // current state of PickupCounterUI
    public enum CounterCurrentState
    {
        GivingOrder, ChoosingNPC
    }

    public static PickupCounterUI Instance;
    public CounterCurrentState currentState;

    public static event System.Action OnCounterClosed;


    // MAY CHANGE LATER
    // public DropSlot targetDropSlot;   // store slot to swap with 
    // public bool isHoveringOnSlot = false;


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


    // set buttons active
   public void SetButtonsActive()
    {
        Next_PreviousManager.Instance.SetCurrent(this);    // give ref to itself
        previousButton.SetActive(true);
        nextButton.SetActive(true);
    }


    // set buttons inactive
    public void SetButtonsInactive()
    {
        previousButton.SetActive(false);
        nextButton.SetActive(false);
        Next_PreviousManager.Instance.SetCurrent(null);  // remove ref
    }


    // if choosing npc go back to choosing order
    public void Previous()
    {
        if (currentState != CounterCurrentState.GivingOrder)
            Debug.Log("Previous pressed");
        else
            Debug.Log("Cannot go back right now.");
    }

    // if choosing order go to choose npc
    public void Next()
    {
        if (currentState == CounterCurrentState.GivingOrder)
        {
            Debug.Log("Next pressed");
        }
    }
}