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


    // call for previous
    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            current?.Previous();
        }
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            current?.Next();
        }
    }
}
