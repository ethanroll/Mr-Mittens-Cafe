using UnityEngine;
using UnityEngine.EventSystems;

public class IceMachineClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public static IceMachineClick Instance { get; private set; }

    [SerializeField] private IceMachine iceMachine; // ref to ice machine

    private bool mouseHeld = false;
    private float mouseHeldTimer = 0f;

    [SerializeField] private float holdTimeLimit = 12f;
    [SerializeField] private float gracePeriod = 2f;
    // [SerializeField] private float threshold = 4f;
    public bool finishedScooping = false;

    private float[] indicationLinePositions = { 0.25f, 0.50f, 1.00f };

    void Awake()
    {
        Instance = this;
    }

    // check mouse states
    public void OnPointerDown(PointerEventData eventData)
    {
        mouseHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseHeld = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseHeld = false;
    }

    void Update()
    {
        // check when mouse is held and for how long
        if (mouseHeld && !finishedScooping)
        {
            // mouseHeldTimer = iceMachine.currentDrink.numIce * holdTimeLimit;  // start where left off if drink already has some ice    
            mouseHeldTimer += Time.deltaTime;

            // calculate for filling bar
            float progress = mouseHeldTimer / holdTimeLimit;
            ProgressBarManager.Instance.FillBar(progress);
            iceMachine.currentDrink.numIce = progress;
            
            if (mouseHeldTimer >= holdTimeLimit)
            {
                finishedScooping = true;
                ProgressBarManager.Instance.FillBar(progress);

                // HAVE IF FOR GRACE PERIOD LATER
                iceMachine.ActionFinished(); 
            }
        }

        else if (mouseHeld && finishedScooping)
        {
            ToastManager.Instance.DisplayInteraction("Cup already has enough water!");
        }
    }

    public void ResetValues()
    {
        mouseHeldTimer = 0f;
        finishedScooping = false;
    }

    // show indication lines
    public void DisplayIndicationLines()
    {
        ProgressBarManager.Instance.hasIndicationLine = true;
        ProgressBarManager.Instance.SetCustomIndicationLines(indicationLinePositions);// set indication line values                                                                             // display progress bar
        ProgressBarManager.Instance.SetProgressBarActive();
    }

    // assign ice level
    public void CheckIceLevel()
    {
        float ice = iceMachine.currentDrink.numIce;

        if (ice * holdTimeLimit >= holdTimeLimit)
            iceMachine.currentDrink.iceLevel = IceLevel.Regular;

        else if (ice * holdTimeLimit >= holdTimeLimit / 2)
            iceMachine.currentDrink.iceLevel = IceLevel.Half;

        else if (ice * holdTimeLimit  >= (holdTimeLimit / 2) / 2)
            iceMachine.currentDrink.iceLevel = IceLevel.Quarter;

        else
            iceMachine.currentDrink.iceLevel = null;
    }
}
