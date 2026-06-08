using UnityEngine;

public class MediumHotCup : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite cupIcon;

    public bool CanInteract()
    {
        return true;
        //return !IsOpened;
    }

    public void Interact()
    {
        Debug.Log("Interacted with a medium cup");
        Inventory.Instance.Add(new Drink { itemName = "Medium Cup", cupSize = CupSize.Medium, icon = cupIcon });
    }
}
