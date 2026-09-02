using UnityEngine;
using System.Collections;

public class EspressoMachineFocus : MonoBehaviour
{
    public static EspressoMachineFocus Instance { get; private set; }

    [SerializeField] private EspressoMachine espressoMachine;   // reference to espresso machine

    private bool clickedOnce = false;
    private float pourTimer = 0f;

    [SerializeField] private float oneEspressoShotTimeAmt = 5;
    [SerializeField] private float espressoCap = 15f;
    [SerializeField] private float espressoCapPeriod = 2f;
    private bool finishedPouring = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!clickedOnce && !finishedPouring)
        {
            // Item currentItem = HotbarManager.Instance.UserCurrentHotbarSlot();

            //if (currentItem is Drink drink && HotbarManager.Instance.hasSlot)
            //{

            // fill withespresso
            pourTimer += Time.deltaTime;

            // calculate for filling bar
            if (pourTimer <= espressoCap)
            {
                ProgressBarManager.Instance.FillBar(pourTimer/espressoCap);
            }
            // grace period before overflow
            //else if (pourTimer >= milkCap + milkCapGracePeriod)
            //{
                // drink.milkOverflow = true;
            //}
            //}
        }

        else if (clickedOnce && !finishedPouring)
        {
            espressoMachine.ActionFinished();
            finishedPouring = true;
        }
    }

    public void OnEspressoMachineButtonClicked()
    {
        // true if first click
        if (!clickedOnce)
        {
            clickedOnce = true;
            StartCoroutine(AddEspresso());
        }
    }

    public void ResetValues()
    {
        pourTimer = 0f;
        clickedOnce = false;
        finishedPouring = false;
    }

    // add num espressos
    private IEnumerator AddEspresso()
    {
        for (int i = 0; i < espressoCap / oneEspressoShotTimeAmt; i++) {
            yield return new WaitForSeconds(5f);
            espressoMachine.currentDrink.numEspressoShots++;
        }
    }
}
