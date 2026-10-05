using UnityEngine;

public class PickupCounter : MonoBehaviour, IInteractable
{
    public static PickupCounter Instance;

    public static event System.Action OnCounterClosed;

    [SerializeField] private GameObject PickupCounterUIParent;
    public bool isOrderComplete = false;

    void Awake()
    {
        Instance = this;
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

        // UI to place order active
        PickupCounterUIParent.SetActive(true);
        CancelManager.Instance.cancelButton.gameObject.SetActive(true);



        // checkingOrder = true;
        // OrderListUI.Instance.PrintAllActiveOrders();

        // get order
    }
}
