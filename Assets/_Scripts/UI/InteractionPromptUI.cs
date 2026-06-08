using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject promptWindow;
    [SerializeField] private TextMeshProGUI promptText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseWindow();
    }

    public void Display(string text)
    {
        promptText.text = text;
        promptWindow.SetActive(true);
    }

    public void CloseWindow()
    {
        promptWindow.SetActive(false);
    }

}
