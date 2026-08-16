using UnityEngine;

public class CancelManager : MonoBehaviour
{
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
}
