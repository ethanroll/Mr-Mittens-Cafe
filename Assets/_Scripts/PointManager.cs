using System;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;
    public static event Action<int> OnScoreChanged; // event for when score changes

    private int score;
    public int currentScore => score; // read the score

    void Awake()
    {
        Instance = this;
    }

    // add to the score
    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }
}
