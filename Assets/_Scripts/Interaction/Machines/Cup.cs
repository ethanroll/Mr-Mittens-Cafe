using UnityEngine;

public class Cup : MonoBehaviour, IInteractable, IPromptable
{
    [SerializeField] private Sprite cupIcon;    // have to change icon for different sizes/type
    private string promptMessage1 = "Would you like a cup for iced drinks or hot drinks?";
    private string promptMessage2 = "Would you like a small, medium or, large cup?";

    private string[] responses1 = new string[] { "Hot", "Iced" };
    private string[] responses2 = new string[] { "Small", "Medium", "Large", "XLarge" };

    private Drink currentDrink;

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        if (!HotbarManager.Instance.IsArrayFull())
        {
            InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage1, responses = responses1 });
            InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage2, responses = responses2 });
            currentDrink = new Drink();
            currentDrink.icon = cupIcon;

            InteractionPromptManager.Instance.LoadPrompt(this);          
        }
        else
        {
            ToastManager.Instance.DisplayInteraction("Can't add anymore items");
        }
    }

    // add drink when all prompts are done
    public void PromptFinished()
    {
        HotbarManager.Instance.AddToHotbar(currentDrink);
        Inventory.Instance.Add(currentDrink);
    }

    // check responses of user input
    public void CheckResponse(string capturedResponse)
    {
        switch (capturedResponse) {
            case "Hot": currentDrink.temperature = Temperature.Hot; break;
            case "Iced": currentDrink.temperature = Temperature.Iced; break;
            case "Small": currentDrink.cupSize = CupSize.Small; break;
            case "Medium": currentDrink.cupSize = CupSize.Medium; break;
            case "Large": currentDrink.cupSize = CupSize.Large; break;
            case "XLarge": currentDrink.cupSize = CupSize.XLarge; break;
        }
    }
}
