using System.ComponentModel.Design.Serialization;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InteractionPromptManager : MonoBehaviour
{
    public static InteractionPromptManager Instance;
    public List<PromptData> prompts; // list that stores prompts for each interactable

    private string interactionPrompt;
    


    private int currentPromptIndex = 0;

    public void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // add data to prompts list
    public void AddPromptData(PromptData promptdata)
    {
        prompts.Add(promptdata);
    }

    // go through prompt system
    public void LoadPrompt()
    {
        for (int i = 0; i < prompts.Count; i++)
        {
            interactionPrompt = prompts[i].promptText;
            // get user click
        }
    }
}
