using UnityEngine;

public class ToppingStationClick : MonoBehaviour
{
    public static ToppingStationClick Instance;
    [SerializeField] private ToppingStation toppingStation; // reference to water machine
    [SerializeField] private GameObject currentToppingSprite;  

    private bool clickedOnce = false;

        void Awake()
    {
        Instance = this;
    }

    // append the topping sprite to drink
    private void AppendToppingSprite()
    {
        
    }
}
