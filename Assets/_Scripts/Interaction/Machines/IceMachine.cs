using UnityEngine;

public class IceMachine : MonoBehaviour, IInteractable, IPromptable, ICurrentMachine
{
    [SerializeField] private Sprite iceMachineIcon;
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject iceMachineUI;

    private MachineState currentState = MachineState.Idle;

    private string promptMessage = "How much ice would you like to fill the cup?";
    private string[] responses = new string[] { "Quarter", "Half", "Regular" };

    public Drink currentDrink; // store drink at current hotbar slot

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        // need to fix: will start when have drink but not selected
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

                /*
                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = responses });
                InteractionPromptManager.Instance.LoadPrompt(this); */
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

        IceMachineClick.Instance.CheckIceLevel();
        Debug.Log(currentDrink.iceLevel);
        ToastManager.Instance.DisplayInteraction("Added ice into the cup.");
        // Debug.Log($"espresso shots: {currentDrink.numEspressoShots}");
    }


    public void OnFocusExit()
    {
        // reset/set values for icemachineclick
        IceMachineClick.Instance.ResetValues();
        ActionFinished();

        // remove UI
        machineFocusParent.SetActive(false);
        iceMachineUI.SetActive(false);
        MachineFocusManager.Instance.cancelButton.gameObject.SetActive(false);

        ProgressBarManager.Instance.SetProgressBarInactive();
        ProgressBarManager.Instance.SetBarAmount(0f);

        PlayerMovement.Instance.canMove = true;
    }

    public void PromptFinished()
    {
        ToastManager.Instance.DisplayInteraction("Added ice to cup.");
        HotbarManager.Instance.GetCurrentItemName(currentDrink);
    }

    public void CheckResponse(string capturedResponse)
    {
        switch (capturedResponse)
        {
            case "Quarter": currentDrink.iceLevel = IceLevel.Quarter; break;
            case "Half": currentDrink.iceLevel = IceLevel.Half; break;
            case "Regular": currentDrink.iceLevel = IceLevel.Regular; break;
        }
    }
}
