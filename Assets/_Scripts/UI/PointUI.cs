using UnityEngine;
using TMPro;

public class PointUI : MonoBehaviour
{
    [SerializeField] private GameObject pointBacking;
    [SerializeField] private TMP_Text scoreText;

    // subscribe
    void OnEnable()
    {
        PointManager.OnScoreChanged += UpdateScoreText;
    }

    // unsubscribe
    void OnDisable()
    {
        PointManager.OnScoreChanged -= UpdateScoreText;
    }

    // run when event fires, update score UI
    void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }
}
