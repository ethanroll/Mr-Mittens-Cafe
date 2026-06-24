using UnityEngine;

public class IceMachine : MonoBehaviour, IInteractable, IPromptable
{
    [SerializeField] private Sprite iceMachineIcon;
    private string promptMessage = "How much ice would you like to fill the cup?";

    private string[] responses = new string[] { "Quarter", "Half", "Regular" };
    private Drink currentDrink; // store drink at current hotbar slot

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        // need to fix: will start when have drink but not selected
        Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot
        if (currentItem is Drink drink && HotbarManager.Instance.pressedOnce)
        {
            if (!drink.hasIce)  // check if drink has ice already
            {
                currentDrink = drink; // store reference for CheckResponse to use
                InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage, responses = responses });
                InteractionPromptManager.Instance.LoadPrompt(this);
                drink.hasIce = true;
            }
            else
            {
                ToastManager.Instance.DisplayMessage("Drink already has ice");
            }
        }
        else
        {
            ToastManager.Instance.DisplayMessage("No drink selected");
        }
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
