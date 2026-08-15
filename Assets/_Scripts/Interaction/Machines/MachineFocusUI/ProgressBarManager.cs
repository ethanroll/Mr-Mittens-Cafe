using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public static ProgressBarManager Instance;

    [SerializeField] private Image progressBarFill;
    [SerializeField] private GameObject progressBar;

    void Awake()
    {
        Instance = this;
    }

    public void FillBar(float fillAmt)
    {
        progressBarFill.fillAmount = fillAmt;
    }

    public void SetBarAmount(float setBarAmt)
    {
        progressBarFill.fillAmount = setBarAmt;
    }

    public void SetProgressBarActive()
    {
        progressBar.transform.parent.gameObject.SetActive(true);
    }

    public void SetProgressBarInactive()
    {
        progressBar.transform.parent.gameObject.SetActive(false);
    }
}
