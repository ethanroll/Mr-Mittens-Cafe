using UnityEngine;

public class ApplePie : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite applePieIcon;

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        if (!HotbarManager.Instance.IsArrayFull()) {
            ToastManager.Instance.DisplayInteraction("You grabbed an Apple Pie");

            Food applePie = new Food();

            applePie.icon = applePieIcon;
            applePie.itemName = "Apple Pie";
            applePie.pastryType = PastryType.ApplePie;

            HotbarManager.Instance.AddToHotbar(applePie);
            Inventory.Instance.Add(applePie);
        }
        else
        {
            ToastManager.Instance.DisplayInteraction("Can't add anymore items");
        }
    }
}
