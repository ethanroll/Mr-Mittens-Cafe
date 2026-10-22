using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PickupCounter : MonoBehaviour, IInteractable, ICancellable, INext_Previous
{    
    public static PickupCounter Instance;

    // current state of PickupCounterUI
    public enum CounterCurrentState
    {
        GivingOrder, ChoosingNPC
    }
    public CounterCurrentState currentState;

    public static event System.Action OnCounterClosed;

    [SerializeField] private GameObject PickupCounterUIParent;
    [SerializeField] private GameObject DragAndDropArea;
    [SerializeField] private GameObject dropSlotPrefab;
    [SerializeField] private GameObject FinishButton;

    [SerializeField] private GameObject buttonParent;
    /*[SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject nextButton; */

    [SerializeField] private GameObject npcList;

    // public List<DropSlot> dropSlotArr = new List<DropSlot>();
    public bool canDragIcons = false;
    public bool isOrderComplete = false;

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        // instantiate 3 drop slots
        for (int i = 0; i < 3; i++)
        {
            GameObject dropSlot = Instantiate(dropSlotPrefab, DragAndDropArea.transform);

            dropSlot.GetComponent<DropSlot>().dropSlotIndex = i;
        }
    }


    public void PickupCounterUICancel()
    {
        // reset values if user cancel order before completing
        if (!isOrderComplete)
        {
            // fire event when close
            OnCounterClosed?.Invoke();
        }
    }

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }


    public void Interact()
    {
        // 1. prompt shows up, place order
        // 2. prompt again if more than 1
        // 3. user click done placing order
        // 4. prompt for which npc order you would like to give

        CancelManager.Instance.SetCancellable(this);

        // UI to place order active
        SetUIActive();
        
        // set state to giving order first
        currentState = CounterCurrentState.GivingOrder;

        canDragIcons = true;


        // checkingOrder = true;
        // OrderListUI.Instance.PrintAllActiveOrders();

        // get order
    }


    // set buttons active
    public void SetUIActive()
    {
        PickupCounterUIParent.SetActive(true);
        CancelManager.Instance.cancelButton.gameObject.SetActive(true);

        Next_PreviousManager.Instance.SetCurrent(this);    // give ref to itself
        buttonParent.SetActive(true);
    }


    // set buttons inactive
    public void SetUIInactive()
    {
        Next_PreviousManager.Instance.SetCurrent(null);  // remove ref

        PickupCounterUIParent.SetActive(false);

        if (currentState == CounterCurrentState.ChoosingNPC)
        {
            HUDManager.Instance.SetHUDVisible(true);
            npcList.SetActive(false);
        }

        buttonParent.SetActive(false);
        CancelManager.Instance.cancelButton.gameObject.SetActive(false);
    }

    
    // if choosing npc go back to choosing order
    public void Previous()
    {
        if (currentState == CounterCurrentState.ChoosingNPC)
        {
            PickupCounterUIParent.SetActive(true);
            HUDManager.Instance.SetHUDVisible(true);
            npcList.SetActive(false);

            currentState = CounterCurrentState.GivingOrder;
            Debug.Log("Previous pressed");
        }
        else
        {
            Debug.Log("Cannot go back right now.");
        }
    }

    // if choosing order go to choose npc
    public void Next()
    {
        if (currentState == CounterCurrentState.GivingOrder)
        {
            HUDManager.Instance.SetHUDVisible(false);
            PickupCounterUIParent.SetActive(false);
            npcList.SetActive(true);
            NPCListUI.Instance.PrintAllActiveNPCS();

            currentState = CounterCurrentState.ChoosingNPC;
            Debug.Log("Next pressed");
        }
        else
        {
            Debug.Log("Cannot go back right now.");
        }
    }


    // call cancel for UI
    public void Cancel()
    {
        PickupCounterUICancel();
        SetUIInactive();
    }
}
