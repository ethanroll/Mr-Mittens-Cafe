using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public static ProgressBarManager Instance;

    [SerializeField] private Image progressBarFill;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject indicationLineContainer;

    private List<GameObject> spawnIndicationLine = new List<GameObject>();     // store indication lines for progress bar
    [SerializeField] private GameObject indicationLinePrefab;
    private float numIndicationLines = 0;
    public bool hasIndicationLine = false;
    

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

        // check if has indication line
        if (hasIndicationLine)
        {
            for (int i = 1; i <= numIndicationLines; i++)
            {
                /* GameObject indicationLine = Instantiate(indicationLinePrefab, indicationLineContainer.transform);
                spawnIndicationLine.Add(indicationLine); */

                GameObject line = Instantiate(indicationLinePrefab, indicationLineContainer.transform);
                RectTransform rt = line.GetComponent<RectTransform>();

                // FIX VALS LATER
                float fraction = (i * 5f) / 15f; // exact fill fraction for this shot
                Debug.Log($"Line {i}: fraction = {fraction}");

                rt.anchorMin = new Vector2(0f, fraction);
                rt.anchorMax = new Vector2(1f, fraction);
                rt.anchoredPosition = new Vector2(0, 0);

                spawnIndicationLine.Add(line);
            }
        }
    }

    public void SetProgressBarInactive()
    {
        progressBar.transform.parent.gameObject.SetActive(false);

        if (hasIndicationLine)
        {
            ResetIndicationLines();
        }
    }

    // setup indication amt
    public void SetNumIndicationLines(float dividend, float divisor)
    {
        numIndicationLines = dividend / divisor;    // how many indication lines
    }

    // reset indication line values
    public void ResetIndicationLines()
    {
        numIndicationLines = 0;
        hasIndicationLine = false;

        // despawn all indication lines
        foreach(GameObject obj in spawnIndicationLine)
        {
            Destroy(obj);
        }

        spawnIndicationLine.Clear();
    }
}
