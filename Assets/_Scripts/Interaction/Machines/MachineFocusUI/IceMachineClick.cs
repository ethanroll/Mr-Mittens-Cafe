using UnityEngine;
using UnityEngine.EventSystems;

public class IceMachineClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public static IceMachineClick Instance { get; private set; }

    [SerializeField] private IceMachine iceMachine; // ref to ice machine

    private bool mouseHeld = false;
    private float mouseHeldTimer = 0f;

    [SerializeField] private float holdTimeLimit = 12;
    [SerializeField] private float threshold = 4f;
    public bool finishedScooping = false;

    void Awake()
    {
        Instance = this;
    }

    // check mouse states
    public void OnPointerDown(PointerEventData eventData)
    {
        mouseHeld = true;
        Debug.Log("pointer down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseHeld = false;
        Debug.Log("pointer up");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseHeld = false;
        Debug.Log("pointer exit");
    }

    void Update()
    {
        // check when mouse is held and for how long
        // check when mouse is held and for how long
        if (mouseHeld && !finishedPouring)
        {
            mouseHeldTimer = iceMachine.currentDrink.waterFillProgress * holdTimeLimit;  // start where left off if drink already has some water    
            mouseHeldTimer += Time.deltaTime;

            // calculate for filling bar
            float progress = mouseHeldTimer / holdTimeLimit;
            ProgressBarManager.Instance.FillBar(progress);
            iceMachine.currentDrink.waterFillProgress = progress;

            if (mouseHeldTimer >= holdTimeLimit)
            {
                finishedPouring = true;
                ProgressBarManager.Instance.FillBar(progress);
                iceMachine.ActionFinished();
            }
        }

        else if (mouseHeld && finishedPouring)
        {
            ToastManager.Instance.DisplayInteraction("Cup already has enough water!");
        }
    }

    public void ResetValues()
    {
        mouseHeldTimer = 0f;
        finishedScooping = false;
    }
}
