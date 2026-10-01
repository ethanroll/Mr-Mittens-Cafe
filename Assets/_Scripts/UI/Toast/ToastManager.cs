using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance;

    [SerializeField] private Toast equippedToast;
    [SerializeField] private Toast interactionToast;

    void Awake()
    {
        Instance = this;
    }

    public void DisplayEquippedItem(string message)
    {
        equippedToast.Show(message);
    }

    public void DisplayInteraction(string message)
    {
        interactionToast.Show(message);
    }
}