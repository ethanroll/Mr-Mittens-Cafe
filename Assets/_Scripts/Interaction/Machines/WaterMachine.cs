using System.Collections;
using UnityEngine;

public class WaterMachine : MonoBehaviour, IInteractable, ICurrentMachine //, IPromptable
{
    [SerializeField] private Sprite WaterMahineIcon;
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject waterMachineUI;

    private Drink currentDrink; // store drink at current hotbar slot

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot

        if(currentItem is Drink drink && HotbarManager.Instance.hasSlot)
        {
            if(!drink.waterFilled) // check if cup is a mug and can take water
            {
                MachineFocusManager.Instance.SetCurrentMachine(this);   // give machinefocusmanager a reference to itself

                currentDrink = drink;   // store reference for CheckResponse to use

                // start pouring water minigame
                ToastManager.Instance.DisplayInteraction("Hold the button to begin pouring water.");
                machineFocusParent.SetActive(true);
                waterMachineUI.SetActive(true);
                MachineFocusManager.Instance.cancelButton.gameObject.SetActive(true);

                ProgressBarManager.Instance.SetProgressBarActive();
                ProgressBarManager.Instance.SetBarAmount(drink.waterFillProgress);

                PlayerMovement.Instance.canMove = false;
            }
            else
            {
                ToastManager.Instance.DisplayInteraction("Cup already has water in it.");
            }
        }
        else
        {
            ToastManager.Instance.DisplayInteraction("No drink selected");
        }
    }

    public void ActionFinished()
    {
        // wait til player done interacting
        if (WaterMachineClick.Instance.finishedPouring)
        {
            currentDrink.waterFilled = true;
            ToastManager.Instance.DisplayInteraction("Finished Pouring water into the cup.");
        }
    }

    // if press esc or press cancel
    public void OnFocusExit()
    {
        // reset values for watermachineclick
        WaterMachineClick.Instance.ResetValues();

        // remove UI
        machineFocusParent.SetActive(false);
        waterMachineUI.SetActive(false);
        MachineFocusManager.Instance.cancelButton.gameObject.SetActive(false);

        ProgressBarManager.Instance.SetProgressBarInactive();
        ProgressBarManager.Instance.SetBarAmount(0f);

        PlayerMovement.Instance.canMove = true;
    }
}
