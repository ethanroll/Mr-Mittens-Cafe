using System.Collections;
using UnityEngine;

public class WaterMachine : MonoBehaviour, IInteractable, IPromptable
{
    [SerializeField] private Sprite WaterMahineIcon;
    private string promptMessage = "Would you like to add water?";
    private string[] responses = new string[] { "Yes", "No" };

    private Drink currentDrink; // store drink at current hotbar slot
    private bool responseYes;

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
            if(!drink.hasWater) // check if cup is a mug and can take water
            {
                currentDrink = drink;   // store reference for CheckResponse to use

                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = responses });
                InteractionPromptManager.Instance.LoadPrompt(this);
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

    public void PromptFinished()
    {
        StartCoroutine(PourWater());
    }

    public void CheckResponse(string capturedResponse)
    {
        switch (capturedResponse)
        {
            case "Yes": 
                currentDrink.hasWater = true;
                break;

            case "No": 
                currentDrink.hasWater = false;
                break;
        }
    }

    private IEnumerator PourWater()
    {
        // FIX LATER SINCE DATA IS ALREADY ADDED BEFORE BREWING IE IF PLAAYER WALKS AWAY OR CANCELS ETC
        HotbarManager.Instance.drinkIsBusy = true;

        ToastManager.Instance.DisplayInteraction("Pouring water into the cup.");
        yield return new WaitForSeconds(4f);
        ToastManager.Instance.DisplayInteraction("Finished Pouring water into the cup.");
        HotbarManager.Instance.GetCurrentItemName(currentDrink);

        HotbarManager.Instance.drinkIsBusy = false;
    }
}
