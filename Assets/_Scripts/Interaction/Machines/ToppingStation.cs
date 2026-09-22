using System;
using UnityEngine;
using UnityEngine.UI;


public class ToppingStation : MonoBehaviour, IInteractable, ICurrentMachine
{
    [SerializeField] private GameObject machineFocusParent;
    [SerializeField] private GameObject toppingStationUI;
    [SerializeField] private GameObject toppingStationPrefabParent;
    [SerializeField] private GameObject toppingSlotPrefab;

    private Sprite currentSprite;
    

    // have current prefab

    private MachineState currentState = MachineState.Idle;
    
    public Drink currentDrink; // store drink at current hotbar slot

    void Start()
    {
        // initiate all topping prefabs
        foreach (DrinkTopping drinkTopping in Enum.GetValues(typeof(DrinkTopping)))
        {
            GameObject toppingPrefabObj = Instantiate(toppingSlotPrefab, toppingStationPrefabParent.transform);

            // assign sprite
            Sprite currentSprite = ToppingStationUIManager.Instance.GetSprite(drinkTopping);
            Image iconImage = toppingPrefabObj.transform.Find("itemIcon").GetComponent<Image>();
            iconImage.sprite = currentSprite;

            // assign topping type
            ToppingStationPrefab prefabScript = toppingPrefabObj.GetComponent<ToppingStationPrefab>();
            prefabScript.assignedTopping = drinkTopping;
        }
    }

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot(); // returns Item at currentHotbarSlot

        if (currentItem is Drink drink && HotbarManager.Instance.hasSlot)
        {
            if (drink.iceLevel == null)  // check if drink has ice already
            {
                currentDrink = drink; // store reference for CheckResponse to use

                PlayerMovement.Instance.canMove = false;

                MachineFocusManager.Instance.SetCurrentMachine(this);

                // display topping station UI
                machineFocusParent.SetActive(true);
                toppingStationUI.SetActive(true);
                CancelManager.Instance.cancelButton.gameObject.SetActive(true);
            }
            else
            {
                ToastManager.Instance.DisplayInteraction("Drink already has toppings");
            }
        }
        else
        {
            ToastManager.Instance.DisplayInteraction("No drink selected");
        }
    }
    

    public void ActionFinished()
    {

    }


    public void OnFocusExit()
    {
        // remove UI
        machineFocusParent.SetActive(false);
        toppingStationUI.SetActive(false);
        CancelManager.Instance.cancelButton.gameObject.SetActive(false);

        ProgressBarManager.Instance.SetProgressBarInactive();

        PlayerMovement.Instance.canMove = true;
    }

    // function
        // havve list and can choose topping
}
