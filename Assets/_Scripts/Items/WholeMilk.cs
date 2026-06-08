using UnityEngine;

public class Milk : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite milkIcon;

    public bool CanInteract()
    {
        return true;
        //return !IsOpened;
    }

    public void Interact()
    {

        Debug.Log("Interacted with whole milk");

       // Inventory.Instance.Add(new Item { itemName = "Whole Milk", type = ItemType.Milk, quantity = 1 });

    }
}
