using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [SerializeField] TextMeshProUGUI TimerText;

    private float timeLimit = 5f;
    public bool isRoundOver = false;

    private float mins;
    private float secs;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isRoundOver)
        {
            if (timeLimit > 0)
            {
                // subtract the time passed since the last frame, deltatime = 0.016 of a second
                timeLimit -= Time.deltaTime;

                if(timeLimit < 0)
                    timeLimit = 0;

                ConvertTime();
                TimerText.text = string.Format("{0:00}:{1:00}", mins, secs);
            }
            else
            {
                TimerText.text = "Time ran out!";
                isRoundOver = true;
            }
        }
    }

    private void ConvertTime()
    {
        mins = Mathf.FloorToInt(timeLimit / 60);
        secs = Mathf.FloorToInt(timeLimit % 60);
    }
}
