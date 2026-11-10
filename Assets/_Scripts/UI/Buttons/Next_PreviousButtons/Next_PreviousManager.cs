using UnityEngine;
using UnityEngine.InputSystem;

public class Next_PreviousManager : MonoBehaviour
{
    public static Next_PreviousManager Instance;

    private INext_Previous current;
    public void SetCurrent(INext_Previous prompt) => current = prompt;


    void Awake()
    {
        Instance = this;
    }

    public void PreviousPressed() => current?.Previous();
    public void NextPressed() => current?.Next();
    public void FinishPressed() => current?.Finish();


    // call for previous
    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            current?.Previous();
        }
    }

    // call for next
    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            current?.Next();
        }
    }

    // call for finish
    public void OnFinish(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            current?.Finish();
        }
    }
}
