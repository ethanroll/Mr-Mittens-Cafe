using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MilkDispenserClick : MonoBehaviour
{
    public static MilkDispenserClick Instance { get; private set; }

    [SerializeField] private MilkDispenser milkDispenser;   // reference to milk dispenser

    private bool clickedOnce = false;
    private float pourTimer = 0f;

    [SerializeField] private float milkCap = 5f;
    [SerializeField] private float milkCapGracePeriod = 2f;
    private bool finishedPouring = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!clickedOnce && !finishedPouring)
        {
            Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot();

            if (currentItem is Drink drink && HotbarManager.Instance.hasSlot)
            {
                // fill with milk
                pourTimer += Time.deltaTime;
                drink.milkFillProgress = pourTimer / milkCap;

                // calculate for filling bar
                if (pourTimer <= milkCap)
                {
                    ProgressBarManager.Instance.FillBar(drink.milkFillProgress);
                }
                // grace period before overflow
                else if (pourTimer >= milkCap + milkCapGracePeriod)
                {
                    drink.milkOverflow = true;
                }
            }
        }

        else if(clickedOnce && !finishedPouring)
        {
            milkDispenser.ActionFinished();
            finishedPouring = true;
        }
    }

    public void OnMilkDispenserButtonClicked()
    {
        // true if first click
        if (!clickedOnce)
        {
            clickedOnce = true;
        }
    }

    public void ResetValues()
    {
        pourTimer = 0f;
        clickedOnce = false;
        finishedPouring = false;
    }
}
