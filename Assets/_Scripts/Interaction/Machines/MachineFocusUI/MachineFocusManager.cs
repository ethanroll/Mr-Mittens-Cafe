using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MachineFocusManager : MonoBehaviour, ICancellable
{
    public static MachineFocusManager Instance;
    private ICurrentMachine currentMachine = null; // hold reference to the current machine calling it

    [SerializeField] public Button cancelButton;

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
        CancelManager.Instance.SetCancellable(this);
    }

    // if press the cancel button
    public void Cancel()
    {
        currentMachine?.OnFocusExit();
    }
}
