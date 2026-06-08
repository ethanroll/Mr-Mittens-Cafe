using UnityEngine;

public interface IInteractable
{
    void Interact();            // prompt interaction
    bool CanInteract();
    string GetInteractionPrompt();  // prompt text
}
