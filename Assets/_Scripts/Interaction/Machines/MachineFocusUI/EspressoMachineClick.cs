using UnityEngine;
using System.Collections;

public class EspressoMachineClick : MonoBehaviour
{
    public static EspressoMachineClick Instance { get; private set; }

    [SerializeField] private EspressoMachine espressoMachine;   // reference to espresso machine

    private bool clickedOnce = false;
    private float pourTimer = 0f;

    [SerializeField] private float oneEspressoShotTimeAmt = 5;
    [SerializeField] private float espressoCap = 15f;
    [SerializeField] private float espressoCapGracePeriod = 2f;

    private bool finishedPouring = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // start machine
        if(espressoMachine.currentState == MachineState.Active)
        {
            StartTimer();
        }

        // stop machine
        else if (espressoMachine.currentState == MachineState.Inactive){
            finishedPouring = true;
            espressoMachine.ActionFinished();
            StopAllCoroutines();
            espressoMachine.currentState = MachineState.Idle;
        }
    }

    public void OnEspressoMachineButtonClicked()
    {
        // true if first click start espresso machine
        if (!clickedOnce)
        {
            // set indiciation lines on progress bar
            ProgressBarManager.Instance.hasIndicationLine = true;
            ProgressBarManager.Instance.SetNumIndicationLines(espressoCap, oneEspressoShotTimeAmt);// set indication line values                                                                             // display progress bar
            ProgressBarManager.Instance.SetProgressBarActive();

            clickedOnce = true;
            espressoMachine.currentState = MachineState.Active;
            StartCoroutine(AddEspresso());
        }
        else
        {
            espressoMachine.currentState = MachineState.Inactive;
        }
    }

    public void ResetValues()
    {
        pourTimer = 0f;
        clickedOnce = false;
        finishedPouring = false;
    }


    // timer when to start espresso machine
    private void StartTimer()
    {
        // fill withespresso
        pourTimer += Time.deltaTime;
        espressoMachine.currentDrink.excessEspresso += Time.deltaTime;

        // calculate how much excess between shots
        if (espressoMachine.currentDrink.excessEspresso >= oneEspressoShotTimeAmt)
        {
            espressoMachine.currentDrink.excessEspresso = 0;;     
        }

        // calculate for filling bar
        if (pourTimer <= espressoCap)
        {
            ProgressBarManager.Instance.FillBar(pourTimer/espressoCap);
        }
    }


    // add num espressos
    private IEnumerator AddEspresso()
    {
        for (int i = 0; i < espressoCap / oneEspressoShotTimeAmt; i++) {
            yield return new WaitForSeconds(oneEspressoShotTimeAmt);
            espressoMachine.currentDrink.numEspressoShots++;
            Debug.Log(espressoMachine.currentDrink.numEspressoShots);
        }
    }
}


