using UnityEngine;

public class ToppingStation : MonoBehaviour, IInteractable, ICurrentMachine
{
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject toppingStationUI;
    [SerializeField] private GameObject toppingSlotPrefab;

    // have current prefab

    private MachineState currentState = MachineState.Idle;
    
    public Drink currentDrink; // store drink at current hotbar slot



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

                // display ice mahine UI
                machineFocusParent.SetActive(true);
                iceMachineUI.SetActive(true);
                MachineFocusManager.Instance.cancelButton.gameObject.SetActive(true);

                IceMachineClick.Instance.DisplayIndicationLines();
            }
            else
            {
                ToastManager.Instance.DisplayInteraction("Drink already has ice");
            }
        }
        else
        {
            ToastManager.Instance.DisplayInteraction("No drink selected");
        }
    }

    // function
        // havve list and can choose topping
}
