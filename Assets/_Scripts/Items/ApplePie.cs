using UnityEngine;

public class ApplePie : MonoBehaviour, IInteractable
{
    public bool CanInteract()
    {
        return true;
        //return !IsOpened;
    }

    public void Interact()
    {
        Debug.Log("Picked up the Apple Pie");

        //Inventory.Instance.Add(new Item { itemName = "Apple Pie", type = ItemType.Pastry, quantity = 1 });

    }
}
