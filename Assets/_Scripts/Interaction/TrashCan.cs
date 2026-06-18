using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractable
{
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        Inventory.Instance.Remove();
    }
}
