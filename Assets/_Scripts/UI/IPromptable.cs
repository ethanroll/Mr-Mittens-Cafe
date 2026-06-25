using UnityEngine;

public interface IPromptable
{
    void CheckResponse(string capturedResponse);       // check response from prompts
    void PromptFinished();
}
