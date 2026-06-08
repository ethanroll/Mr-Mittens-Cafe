using UnityEngine;

public class LargeHotCup : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite cupIcon;

    public bool CanInteract()
    {
        return true;
        //return !IsOpened;
    }

    public void Interact()
    {
        Debug.Log("Interacted with a large cup");
        Inventory.Instance.Add(new Drink { itemName = "Large Cup", cupSize = CupSize.Large, icon = cupIcon });
    }
}
