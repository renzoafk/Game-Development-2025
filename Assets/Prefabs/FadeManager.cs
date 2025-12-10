using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private CanvasGroup CanvasGroup;

    [SerializeField] private bool automaticallyFadeInONSceneLoad;
    [SerializeField] private float automaticFadeStartAlpha = 1f;
    [SerializeField] private float automaticFadeEndAlpha = 0f;
    [SerializeField] private float automaticFadeDuration = 1f;
    [SerializeField] private float automaticFadeBeforeFade = 0f;

    [Header("UI to Show After Fade")]
    [SerializeField] private GameObject healthBar;   // Drag your HealthBar here

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Hide HealthBar at the start (if assigned)
        if (healthBar != null)
            healthBar.SetActive(false);

        // Automatically fade in if enabled
        if (automaticallyFadeInONSceneLoad)
        {
            DoFade(automaticFadeStartAlpha, automaticFadeEndAlpha, automaticFadeDuration, automaticFadeBeforeFade);
        }
    }

    public void DoFade(float startAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(AnimateFade(startAlpha, endAlpha, duration, delayBeforeFade));
    }

    private IEnumerator AnimateFade(float startAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        fadeImage.enabled = true;
        CanvasGroup.alpha = startAlpha;

        yield return null;

        // Optional delay before beginning fade animation
        yield return new WaitForSeconds(delayBeforeFade);

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float fadePercentage = Mathf.Clamp01(timeElapsed / duration);

            CanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, fadePercentage);
            yield return null;
        }

        CanvasGroup.alpha = endAlpha;

        // Disable fade image if fully transparent at end
        if (endAlpha <= 0f)
            fadeImage.enabled = false;

        // 👉 SHOW HEALTH BAR *after* the fade completes
        if (healthBar != null && endAlpha <= 0f)
            healthBar.SetActive(true);
    }
}
