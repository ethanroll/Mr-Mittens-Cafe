using UnityEngine;

public class SmallCup : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite cupIcon;

    public bool CanInteract()
    {
        return true;
        //return !IsOpened;
    }

    public void Interact()
    {
        Debug.Log("Interacted with a small cup");
        Inventory.Instance.Add(new Drink { itemName = "Small Cup", cupSize = CupSize.Small, icon = cupIcon });
    }
}
