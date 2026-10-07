using UnityEngine;
using System.Collections.Generic;

public class PickupCounter : MonoBehaviour, IInteractable, ICancellable
{
    public static PickupCounter Instance;

    [SerializeField] private GameObject PickupCounterUIParent;
    [SerializeField] private GameObject DragAndDropArea;
    [SerializeField] private GameObject dropSlotPrefab;
    [SerializeField] private GameObject FinishButton;

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

            // give reference to drop slot index
            // dropSlotArr.Add(dropSlot);
            dropSlot.GetComponent<DropSlot>().dropSlotIndex = i;
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
        PickupCounterUIParent.SetActive(true);
        CancelManager.Instance.cancelButton.gameObject.SetActive(true);

        canDragIcons = true;


        // checkingOrder = true;
        // OrderListUI.Instance.PrintAllActiveOrders();

        // get order
    }

    // call cancel for UI
    public void Cancel()
    {
        PickupCounterUI.Instance.PickupCounterUICancel();

        // set ui inactive
        PickupCounterUIParent.SetActive(false);
        CancelManager.Instance.cancelButton.gameObject.SetActive(false);
    }
}
