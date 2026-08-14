using UnityEngine;
using UnityEngine.InputSystem;

public class MachineFocusManager : MonoBehaviour
{
    public static MachineFocusManager Instance;
    private ICurrentMachine currentMachine = null; // hold reference to the current machine calling it

    void Awake()
    {
        Instance = this;
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentMachine?.OnFocusExit();
        }
    }

    public void SetCurrentMachine(ICurrentMachine currentMachine)
    {
        this.currentMachine = currentMachine;
    }
}
