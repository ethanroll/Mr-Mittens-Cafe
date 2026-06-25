using UnityEngine;
using System.Collections;
using TMPro;

public class Toast : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI toastText;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float holdDuration = 2f;

    private Coroutine activeRoutine;

    public void Show(string message)
    {
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        toastText.text = message;
        activeRoutine = StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        canvasGroup.blocksRaycasts = true;

        // fade in
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = elapsed / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f; // make sure it lands exactly on 1

        yield return new WaitForSeconds(holdDuration);  // hold duration

        // fade out
        elapsed = 0f; // reset elapsed value
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f; // make sure it lands exactly on 0

        canvasGroup.blocksRaycasts = false;
        activeRoutine = null;
    }
}