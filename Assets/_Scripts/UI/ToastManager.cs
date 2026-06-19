using UnityEngine;
using System.Collections;
using TMPro;


// FIX LATER SET ACTIVE CURRENT ALPHA MEANS UI IS STILL THERE EVEN WHEN ALPHA = 0; CAN INTERRUPT CLICKS

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance;

    [SerializeField] private CanvasGroup ToastNotification;
    [SerializeField] private TextMeshProUGUI ToastNotificationText;

    private float elapsed = 0f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float holdDuration = 2f;

    void Awake()
    {
        Instance = this;
    }

    IEnumerator FadeRoutine()
    {
        // fade in
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            ToastNotification.alpha = elapsed / fadeDuration;
            yield return null;
        }

        ToastNotification.alpha = 1f; // make sure it lands exactly on 1
        yield return new WaitForSeconds(holdDuration);// hold duration
        elapsed = 0f;   // reset elapsed value

        // fade out
        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            ToastNotification.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }
        ToastNotification.alpha = 0f; // make sure it lands exactly on 0
    }

    public void DisplayMessage(string message)
    {
        StopAllCoroutines();
        ToastNotificationText.text = message;
        StartCoroutine(FadeRoutine());
    }
}
