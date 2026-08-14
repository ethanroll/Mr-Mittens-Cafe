using UnityEngine;
using UnityEngine.EventSystems;

public class WaterMachineClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public static WaterMachineClick Instance;
    [SerializeField] private WaterMachine waterMachine; // reference to water machine

    private bool mouseHeld = false;
    private float mouseHeldTimer = 0f;
    [SerializeField] private float holdTimeLimit = 3f;
    public bool finishedPouring = false;

    void Awake()
    {
        Instance = this;
    }

    // check mouse states
    public void OnPointerDown(PointerEventData eventData) {
        mouseHeld = true;
        Debug.Log("pointer down");
    }

    public void OnPointerUp(PointerEventData eventData) {
        mouseHeld = false;
        Debug.Log("pointer up");
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseHeld = false;
        Debug.Log("pointer exit");
    }

    void Update()
    {
        // check when mouse is held and for how long
        if (mouseHeld && !finishedPouring)
        {
            mouseHeldTimer += Time.deltaTime;

            if (mouseHeldTimer >= holdTimeLimit)
            {
                finishedPouring = true;
                waterMachine.ActionFinished();
            }
        }
    }

    // reset to original values when done
    public void ResetValues()
    {
        mouseHeldTimer = 0f;
        finishedPouring = false;
    }
}
