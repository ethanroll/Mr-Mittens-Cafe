using System.Collections;
using UnityEngine;

public class MilkDispenser : MonoBehaviour, IInteractable, IPromptable
{
    [SerializeField] private Sprite espressoMachineIcon;
    private string promptMessage = "Which type of milk would you like to add?";
    private string[] responses = new string[] { "Whole", "Skim", "Oat", "Almond", "Soy", "Coconut" };

    public bool promptFinished = false;
    private Drink currentDrink; // store drink at current hotbar slot

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
            if (!drink.hasMilk)
            {
                currentDrink = drink; // store reference for CheckResponse to use

                promptFinished = false;
                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = responses });
                InteractionPromptManager.Instance.LoadPrompt(this);

                drink.hasMilk = true;
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

    public void PromptFinished()
    {
        promptFinished = true;
        StartCoroutine(DispenseMilk());
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

        ToastManager.Instance.DisplayInteraction("Pouring the milk");
        yield return new WaitForSeconds(4f);
        ToastManager.Instance.DisplayInteraction("Milk poured");

        HotbarManager.Instance.drinkIsBusy = false;
    }
}
