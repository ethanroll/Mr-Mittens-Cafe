using UnityEngine;

public class Cup : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite cupIcon;    // have to change icon for different sizes/type
    private string promptMessage1 = "Would you like an cup for iced drinks or hot drinks?"
    private string promptMessage2 = "Would you like a small, medium or, large cup?"

    private string[] responses1 = new string[] { "Hot", "Iced" };
    private string[] responses2 = new string[] { "Small", "Medium", "Large" };

    // start populating promptdata list
    public void Start()
    {
        InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage1, responses = responses1);
        InteractionPromptManager.Instance.AddPromptData(new PromptData { promptText = promptMessage2, responses = responses2);
    }

    public string GetInteractionPrompt()
    {
        return InteractionPrompt.Instance
    }

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        
    }
}
