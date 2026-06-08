using UnityEngine;

public interface IInteractable
{
    string GetInteractionPrompt();  // prompt text
    bool CanInteract();
    void Interact();            // prompt interaction
}
