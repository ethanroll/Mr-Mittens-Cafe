using System;
using System.Collections;
using UnityEngine;

public class EspressoMachine : MonoBehaviour, IInteractable, ICurrentMachine
{
    [SerializeField] private Sprite espressoMachineIcon;
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject espressoMachineUI;

    public MachineState currentState = MachineState.Idle;

    // IMPLEMENT LATER FOR BEAN EVENT
    // public static event Action<int> OnScoreChanged;

    public Drink currentDrink; // store drink at current hotbar slot

    [SerializeField] private int numBeans = 1000; // initial value of espresso beans (will be updated)
    private int numBeansUsedPerShot = 50;   // for 1 shot usage
    private bool machineEmpty = false;  // value for if numBeans == 0

    private int responsesNewLength; // store length of array when already have espresso

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot
        if (currentItem is Drink drink && HotbarManager.Instance.hasSlot && !HotbarManager.Instance.drinkIsBusy && !machineEmpty)
        {
            currentDrink = drink;

            if(drink.numEspressoShots != 0 || drink.numEspresso != 0)
            {
                ToastManager.Instance.DisplayInteraction("Drink already has espresso in it.");
                return;
            }

            PlayerMovement.Instance.canMove = false;

            MachineFocusManager.Instance.SetCurrentMachine(this);   // give ref to MachineFocusManager

            // display espresso machine UI
            StartCoroutine(StartMachinePrompt());
            machineFocusParent.SetActive(true);
            espressoMachineUI.SetActive(true);
            MachineFocusManager.Instance.cancelButton.gameObject.SetActive(true);

            /*
            else if (currentItem is Drink drink && HotbarManager.Instance.hasSlot && !HotbarManager.Instance.drinkIsBusy && machineEmpty)
            {
                ToastManager.Instance.DisplayInteraction("No more espresso beans in machine, must refill.");
            } */
        }

        else
        {
            ToastManager.Instance.DisplayInteraction("No drink selected");
        }
    }


    // prompt user to press button to start until they have
    private IEnumerator StartMachinePrompt()
    {
        while(currentState == MachineState.Idle)
        {
            ToastManager.Instance.DisplayInteraction("Press the button to start the machine.");
            yield return new WaitForSeconds(5f);
        }
    }


    public void ActionFinished()
    {
        if (currentDrink == null)
        {
            Debug.LogError("currentDrink is null!");
            return;
        }
        if (ToastManager.Instance == null)
        {
            Debug.LogError("ToastManager.Instance is null!");
            return;
        }

        ToastManager.Instance.DisplayInteraction("Added espresso into the cup.");
        Debug.Log($"espresso shots: {currentDrink.numEspressoShots}");
    }


    public void OnFocusExit()
    {
        // reset values for espressomachineclick
        EspressoMachineClick.Instance.ResetValues();

        // remove UI
        machineFocusParent.SetActive(false);
        espressoMachineUI.SetActive(false);
        MachineFocusManager.Instance.cancelButton.gameObject.SetActive(false);

        ProgressBarManager.Instance.SetProgressBarInactive();

        PlayerMovement.Instance.canMove = true;
    }
}
