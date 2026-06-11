using UnityEngine;

public interface IInteractable
{
    void Interact();            // prompt interaction
    bool CanInteract();
    void CheckResponse(string capturedResponse);       // check response from prompts
    void PromptComplete();      // add item when prompt is complete
}
