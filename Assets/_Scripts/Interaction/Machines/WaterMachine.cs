using System.Collections;
using UnityEngine;

public class WaterMachine : MonoBehaviour, IInteractable, ICurrentMachine //, IPromptable
{
    [SerializeField] private Sprite WaterMahineIcon;
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject waterMachineUI;

    //private string promptMessage = "Would you like to add water?";
    //private string[] responses = new string[] { "Yes", "No" };

    private Drink currentDrink; // store drink at current hotbar slot
    //private bool responseYes;

    void Update()
    {

    }

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

                ProgressBarManager.Instance.SetProgressBarActive();
                ProgressBarManager.Instance.SetBarAmount(drink.waterFillProgress);

                PlayerMovement.Instance.canMove = false;

                //InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = responses });
                //InteractionPromptManager.Instance.LoadPrompt(this);
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

    // if press esc
    public void OnFocusExit()
    {
        // reset values for watermachineclick
        WaterMachineClick.Instance.ResetValues();

        // remove UI
        machineFocusParent.SetActive(false);
        waterMachineUI.SetActive(false);

        ProgressBarManager.Instance.SetProgressBarInactive();
        ProgressBarManager.Instance.SetBarAmount(0f);

        PlayerMovement.Instance.canMove = true;
    }



    // NOT NEEDED ANYMORE
    public void PromptFinished()
    {
        StartCoroutine(PourWater());
    }

    public void CheckResponse(string capturedResponse)
    {
        switch (capturedResponse)
        {
            case "Yes": 
                currentDrink.waterFilled = true;
                break;

            case "No": 
                currentDrink.waterFilled = false;
                break;
        }
    }

    private IEnumerator PourWater()
    {
        // FIX LATER SINCE DATA IS ALREADY ADDED BEFORE BREWING IE IF PLAAYER WALKS AWAY OR CANCELS ETC
        HotbarManager.Instance.drinkIsBusy = true;
        PlayerMovement.Instance.canMove = false;

        ToastManager.Instance.DisplayInteraction("Pouring water into the cup.");
        yield return new WaitForSeconds(4f);
        ToastManager.Instance.DisplayInteraction("Finished Pouring water into the cup.");
        HotbarManager.Instance.GetCurrentItemName(currentDrink);

        HotbarManager.Instance.drinkIsBusy = false;
        PlayerMovement.Instance.canMove = true;
    }
}
