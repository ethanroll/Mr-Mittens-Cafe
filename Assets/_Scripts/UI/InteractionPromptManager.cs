using System.Collections.Generic;
//using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class InteractionPromptManager : MonoBehaviour
{
    public static InteractionPromptManager Instance;
    public List<PromptData> prompts; // list that stores prompts for each interactable
    //public bool MouseClicked => Mouse.current.leftButton.wasPressedThisFrame;  // check for user mouse click

    private string interactionPrompt;   // store prompt for each interactable
    private string[] interactionResponse = new string[0]; // store responses for each interactable

    private int currentPromptIndex = 0;

    [SerializeField] private GameObject responseWindowPrefab;
    [SerializeField] private GameObject responseWindowParent;
    [SerializeField] private GameObject promptWindow;        // prompt window parent to get child
    private GameObject responseWindowObj;   // response window parent to get child

    public void Awake()
    {
        Instance = this;
    }

    // add data to prompts list
    public void AddPromptData(PromptData promptdata)
    {
        prompts.Add(promptdata);
    }

    // go through prompt system
    public void LoadPrompt(IInteractable interactable)
    {
        if (currentPromptIndex != prompts.Count)
        {
            removeWindowParentChild();  // remove previous instances of responses

            interactionPrompt = prompts[currentPromptIndex].promptText;
            promptWindow.SetActive(true);
            promptWindow.GetComponentInChildren<TMP_Text>().text = interactionPrompt;

            interactionResponse = prompts[currentPromptIndex].responses;
            for (int i = 0; i < interactionResponse.Length; i++)    // loop through each response and show with UI
            {
                responseWindowObj = Instantiate(responseWindowPrefab, responseWindowParent.transform);      // spawn copy of prefab
                responseWindowObj.GetComponentInChildren<TMP_Text>().text = interactionResponse[i];
                int capturedResponse = i;   // get the response that the user clicked

                // wait for player response
                responseWindowObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    currentPromptIndex++;
                    interactable.CheckResponse(interactionResponse[capturedResponse]);
                    LoadPrompt(interactable);   // loop back for more responses
                });
            }
        }
        else
        {
            // add drink when prompts are done
            interactable.PromptComplete();

            // reset values
            currentPromptIndex = 0;
            prompts.Clear();

            //remove UI
            promptWindow.SetActive(false);
            responseWindowParent.SetActive(false);
            removeWindowParentChild();
        }
    }

    // remove child
    private void removeWindowParentChild()
    {
        foreach (Transform child in responseWindowParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
