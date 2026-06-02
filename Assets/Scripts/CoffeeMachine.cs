using UnityEngine;

public class CoffeeBeans : MonoBehaviour, IInteractable
{

    public bool CanInteract()
    {
        return true;
        //return !IsOpened;
    }
    
    public void Interact()
    {
        Debug.Log("Picked up the Coffee");
 
        Inventory.Instance.Add(new Item { itemName = "Coffee", type = ItemType.Coffee, quantity = 1 });
        
    }
}
