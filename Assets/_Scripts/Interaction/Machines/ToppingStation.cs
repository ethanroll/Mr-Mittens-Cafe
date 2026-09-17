using UnityEngine;
using System;


public class ToppingStation : MonoBehaviour, IInteractable, ICurrentMachine
{
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject toppingStationUI;
    [SerializeField] private GameObject toppingSlotPrefab;
    

    // have current prefab

    private MachineState currentState = MachineState.Idle;
    
    public Drink currentDrink; // store drink at current hotbar slot

    void Start()
    {
        // initiate all topping prefabs
        foreach (DrinkType drink in Enum.GetValues(typeof(DrinkType)))
        {
            GameObject toppingPrefab = Instantiate(toppingSlotPrefab, toppingStationUI.transform);
            // if drink == this assign sprite
    }

    }

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot

        if (currentItem is Drink drink && HotbarManager.Instance.hasSlot)
        {
            if (drink.iceLevel == null)  // check if drink has ice already
            {
                currentDrink = drink; // store reference for CheckResponse to use

                PlayerMovement.Instance.canMove = false;

                MachineFocusManager.Instance.SetCurrentMachine(this);

                // display topping station UI
                machineFocusParent.SetActive(true);
                toppingStationUI.SetActive(true);
                MachineFocusManager.Instance.cancelButton.gameObject.SetActive(true);
            }
            else
            {
                ToastManager.Instance.DisplayInteraction("Drink already has toppings");
            }
        }
        else
        {
            ToastManager.Instance.DisplayInteraction("No drink selected");
        }
    }
    

    public void ActionFinished()
    {

    }


    public void OnFocusExit()
    {
        // remove UI
        machineFocusParent.SetActive(false);
        toppingStationUI.SetActive(false);
        MachineFocusManager.Instance.cancelButton.gameObject.SetActive(false);

        ProgressBarManager.Instance.SetProgressBarInactive();

        PlayerMovement.Instance.canMove = true;
    }

    // function
        // havve list and can choose topping
}
