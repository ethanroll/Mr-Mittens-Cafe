using System.Collections;
using UnityEngine;

public class MilkDispenser : MonoBehaviour, IInteractable, IPromptable, ICurrentMachine
{
    [SerializeField] private Sprite milkDispenserIcon;
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject milkDispenserUI;

    private string promptMessage = "Which type of milk would you like to add?";
    private string[] responses = new string[] { "Whole", "Skim", "Oat", "Almond", "Soy", "Coconut" };

    public MachineState currentState = MachineState.Idle;  // starrting state

    private Drink currentDrink; // store drink at current hotbar slot

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot
        if (currentItem is Drink drink && HotbarManager.Instance.hasSlot && !HotbarManager.Instance.drinkIsBusy)
        {
            if (drink.milkType == null)
            {
                PlayerMovement.Instance.canMove = false;

                MachineFocusManager.Instance.SetCurrentMachine(this);   // give machinefocusmanager a reference to itself

                // change machine state
                currentState = MachineState.Active;
                currentDrink = drink; // store reference for CheckResponse to use

                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = responses });
                InteractionPromptManager.Instance.LoadPrompt(this);
            }
            else
            {
                ToastManager.Instance.DisplayInteraction("Drink already has milk.");
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

        //PointManager.Instance.AddScore(10); // add points
        if (currentDrink.milkFillProgress != 0)
        {
            currentDrink.milkAdded = true;
            ToastManager.Instance.DisplayInteraction("Added milk into the cup.");
            Debug.Log($"amt of milk: {currentDrink.milkFillProgress}");
        } 
    }

    public void OnFocusExit()
    {
        // reset values for watermachineclick
        MilkDispenserClick.Instance.ResetValues();

        // remove UI
        machineFocusParent.SetActive(false);
        milkDispenserUI.SetActive(false);
        MachineFocusManager.Instance.cancelButton.gameObject.SetActive(false);

        ProgressBarManager.Instance.SetProgressBarInactive();
        ProgressBarManager.Instance.SetBarAmount(0f);

        PlayerMovement.Instance.canMove = true;
    }

    public void PromptFinished()
    {
        // display milk dispenser UI
        ToastManager.Instance.DisplayInteraction("Press the button once the progress bar is full.");
        machineFocusParent.SetActive(true);
        milkDispenserUI.SetActive(true);
        MachineFocusManager.Instance.cancelButton.gameObject.SetActive(true);

        // display progress bar
        ProgressBarManager.Instance.SetProgressBarActive();
        // StartCoroutine(DispenseMilk());
    }

    public void CheckResponse(string capturedResponse)
    {
        switch (capturedResponse)
        {
            case "Whole": currentDrink.milkType = MilkType.Whole; break;
            case "Skim": currentDrink.milkType = MilkType.Skim; break;
            case "Oat": currentDrink.milkType = MilkType.Oat; break;
            case "Almond": currentDrink.milkType = MilkType.Almond; break;
            case "Soy": currentDrink.milkType = MilkType.Soy; break;
            case "Coconut": currentDrink.milkType = MilkType.Coconut; break;
        }
    }

    // dispensing milk logic
    private IEnumerator DispenseMilk()
    {
        // FIX LATER SINCE DATA IS ALREADY ADDED BEFORE BREWING IE IF PLAAYER WALKS AWAY OR CANCELS ETC
        HotbarManager.Instance.drinkIsBusy = true;
        PlayerMovement.Instance.canMove = false;

        ToastManager.Instance.DisplayInteraction("Pouring the milk");
        yield return new WaitForSeconds(4f);
        ToastManager.Instance.DisplayInteraction("Milk poured");
        HotbarManager.Instance.GetCurrentItemName(currentDrink);

        HotbarManager.Instance.drinkIsBusy = false;
        PlayerMovement.Instance.canMove = true;
    }
}
