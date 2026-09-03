using System;
using System.Collections;
using UnityEngine;

public class EspressoMachine : MonoBehaviour, IInteractable, IPromptable, ICurrentMachine
{
    [SerializeField] private Sprite espressoMachineIcon;
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject espressoMachineUI;

    public MachineState currentState = MachineState.Idle;

    private string promptMessage = "How many shots of espresso would you like to add";
    private string[] responses = new string[] { "One", "Two", "Three" };

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

            PlayerMovement.Instance.canMove = false;

            MachineFocusManager.Instance.SetCurrentMachine(this);

            // display espresso machine UI
            StartCoroutine(StartMachinePrompt());
            machineFocusParent.SetActive(true);
            espressoMachineUI.SetActive(true);
            MachineFocusManager.Instance.cancelButton.gameObject.SetActive(true);
            /*
            if (drink.numEspressoShots == 0) // check if cup reached maxEspresso
            {
                currentDrink = drink; // store reference for CheckResponse to use

                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = responses });
                InteractionPromptManager.Instance.LoadPrompt(this);
            }

            else if (drink.numEspressoShots < 3)
            {
                // only show responses for how many num espressos you can add
                responsesNewLength = responses.Length - drink.numEspressoShots;
                string[] newResponses = new string[responsesNewLength];
                Array.Copy(responses, newResponses, responsesNewLength);

                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = newResponses });
                InteractionPromptManager.Instance.LoadPrompt(this);
            }

            */

            /*
            else if (currentItem is Drink drink && HotbarManager.Instance.hasSlot && !HotbarManager.Instance.drinkIsBusy && machineEmpty)
            {
                ToastManager.Instance.DisplayInteraction("No more espresso beans in machine, must refill.");
            } */

            /* else
            {
                ToastManager.Instance.DisplayInteraction("Drink already has max number of espresso shots");
            } */
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

        ToastManager.Instance.DisplayInteraction("Added espresso into the cup.");
        Debug.Log($"espresso shots: {currentDrink.numEspressoShots}");
    }

/*
    public void ActionFinished()
    {
        // wait til player done interacting

        //PointManager.Instance.AddScore(10); // add points
        //if (currentDrink.milkFillProgress != 0)
        //{
            // currentDrink.milkAdded = true;
            ToastManager.Instance.DisplayInteraction("Added espresso into the cup.");
            Debug.Log($"espresso shots: {currentDrink.numEspressoShots}");
        //}
    } */

    public void OnFocusExit()
    {
        // reset values for watermachineclick
        EspressoMachineFocus.Instance.ResetValues();

        // remove UI
        machineFocusParent.SetActive(false);
        espressoMachineUI.SetActive(false);
        MachineFocusManager.Instance.cancelButton.gameObject.SetActive(false);

        ProgressBarManager.Instance.SetProgressBarInactive();
        ProgressBarManager.Instance.SetBarAmount(0f);

        PlayerMovement.Instance.canMove = true;
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

    // call brew espresso when prompt complete
    public void PromptFinished()
    {
        StartCoroutine(BrewEspresso());
    }

    public void CheckResponse(string capturedResponse)
    {
        if (currentDrink.numEspressoShots == 0)
        {
            switch (capturedResponse)
            {
                case "One": currentDrink.numEspressoShots = 1; break;
                case "Two": currentDrink.numEspressoShots = 2; break;
                case "Three": currentDrink.numEspressoShots = 3; break;
            }
        }
        else
        {
            switch (capturedResponse)   // add onto numEspressoShots
            {
                case "One": currentDrink.numEspressoShots += 1; break;
                case "Two": currentDrink.numEspressoShots += 2; break;
            }
        }
    }

    private IEnumerator BrewEspresso()
    {
        // FIX LATER SINCE DATA IS ALREADY ADDED BEFORE BREWING IE IF PLAAYER WALKS AWAY OR CANCELS ETC
        HotbarManager.Instance.drinkIsBusy = true;
        PlayerMovement.Instance.canMove = false;

        ToastManager.Instance.DisplayInteraction("Starting the brewing process");
        yield return new WaitForSeconds(4f);
        ToastManager.Instance.DisplayInteraction("Finished brewing");
        HotbarManager.Instance.GetCurrentItemName(currentDrink);

        HotbarManager.Instance.drinkIsBusy = false;
        PlayerMovement.Instance.canMove = true;
    }
}
