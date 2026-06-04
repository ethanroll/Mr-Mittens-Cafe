using UnityEngine;

public class CoffeeMachine : MonoBehaviour, IInteractable
{

    public bool CanInteract()
    {
        return true;
        //return !IsOpened;
    }
    
    public void Interact()
    {   
        Debug.Log("Interacted with Coffee");
 
       // Inventory.Instance.Add(new Item { itemName = "Coffee", type = ItemType.Coffee, quantity = 1 });
        
    }
}
