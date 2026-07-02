using System;
using System.Collections;
using UnityEngine;

public class EspressoMachine : MonoBehaviour, IInteractable, IPromptable
{
    [SerializeField] private Sprite espressoMachineIcon;
    private string promptMessage = "How many shots of espresso would you like to add";
    private string[] responses = new string[] { "One", "Two", "Three" };

    public bool promptFinished = false;
    private Drink currentDrink; // store drink at current hotbar slot

    private int responsesNewLength; // store length of array when already have espresso

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot
        if (currentItem is Drink drink && HotbarManager.Instance.pressedOnce && !HotbarManager.Instance.drinkIsBusy)
        {
            if (drink.numEspressoShots == 0) // check if cup reached maxEspresso
            {
                currentDrink = drink; // store reference for CheckResponse to use

                promptFinished = false;
                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = responses });
                InteractionPromptManager.Instance.LoadPrompt(this);
            }

            else if (drink.numEspressoShots < 3)
            {
                // only show responses for how many num espressos you can add
                responsesNewLength = responses.Length - drink.numEspressoShots;
                string[] newResponses = new string[responsesNewLength];
                Array.Copy(responses, newResponses, responsesNewLength);

                promptFinished = false;
                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = newResponses });
                InteractionPromptManager.Instance.LoadPrompt(this);
            }

            else
            {
                ToastManager.Instance.DisplayInteraction("Drink already has max number of espresso shots");
            }
        }
        else
        {
            ToastManager.Instance.DisplayInteraction("No drink selected");
        }
    }

    // call brew espresso when prompt complete
    public void PromptFinished()
    {
        promptFinished = true;
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

        ToastManager.Instance.DisplayInteraction("Starting the brewing process");
        yield return new WaitForSeconds(4f);
        ToastManager.Instance.DisplayInteraction("Finished brewing");

        HotbarManager.Instance.drinkIsBusy = false;
    }
}
