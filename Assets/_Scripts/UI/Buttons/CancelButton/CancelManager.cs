using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CancelManager : MonoBehaviour
{
    [SerializeField] public Button cancelButton;

    public static CancelManager Instance;
    private ICancellable currentCancellable;

    
    void Awake()
    {
        Instance = this;
    }

    public void SetCancellable(ICancellable target) => currentCancellable = target;

    // call on object w cancel
    public void OnCancelClicked()
    {
        currentCancellable?.Cancel();
        currentCancellable = null;
    }

    // if press esc
    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentCancellable?.Cancel();
        }
    }
}
