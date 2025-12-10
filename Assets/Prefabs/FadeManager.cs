using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private CanvasGroup CanvasGroup;

    [SerializeField] private bool automaticallyFadeInONSceneLoad = true;
    [SerializeField] private float automaticFadeStartAlpha = 1f;
    [SerializeField] private float automaticFadeEndAlpha = 0f;
    [SerializeField] private float automaticFadeDuration = 1f;
    [SerializeField] private float automaticFadeBeforeFade = 0f;

    [Header("UI to Show After Fade")]
    [SerializeField] private GameObject healthBar;   // Drag your HealthBar here

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Hide HealthBar at the start (if assigned)
        if (healthBar != null)
            healthBar.SetActive(false);

        // Automatically fade in if enabled
        if (automaticallyFadeInONSceneLoad)
        {
            DoFade(automaticFadeStartAlpha, automaticFadeEndAlpha,
                   automaticFadeDuration, automaticFadeBeforeFade);
        }
        else
        {
            // If not auto fading, just make sure fade image is off if alpha is zero
            if (CanvasGroup != null && CanvasGroup.alpha <= 0f && fadeImage != null)
            {
                fadeImage.enabled = false;
            }
        }
    }

    // Old style entry point for simple one off fades
    public void DoFade(float startAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(startAlpha, endAlpha, duration, delayBeforeFade));
    }

    // New: use this inside coroutines where you want to wait for the fade to finish
    public IEnumerator FadeToBlack(float duration, float delayBeforeFade = 0f)
    {
        if (CanvasGroup == null)
            yield break;

        float startAlpha = CanvasGroup.alpha;
        yield return FadeRoutine(startAlpha, 1f, duration, delayBeforeFade);
    }

    public IEnumerator FadeFromBlack(float duration, float delayBeforeFade = 0f)
    {
        if (CanvasGroup == null)
            yield break;

        float startAlpha = CanvasGroup.alpha;
        yield return FadeRoutine(startAlpha, 0f, duration, delayBeforeFade);
    }

    // Shared animation logic
    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        if (fadeImage != null)
            fadeImage.enabled = true;

        if (CanvasGroup != null)
            CanvasGroup.alpha = startAlpha;

        yield return null;

        // Optional delay before beginning fade animation
        if (delayBeforeFade > 0f)
            yield return new WaitForSeconds(delayBeforeFade);

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float fadePercentage = Mathf.Clamp01(timeElapsed / duration);

            if (CanvasGroup != null)
                CanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, fadePercentage);

            yield return null;
        }

        if (CanvasGroup != null)
            CanvasGroup.alpha = endAlpha;

        // Disable fade image if fully transparent at end
        if (endAlpha <= 0f && fadeImage != null)
            fadeImage.enabled = false;

        // Show HealthBar after a fade in to fully visible game
        if (healthBar != null && endAlpha <= 0f)
            healthBar.SetActive(true);

        fadeCoroutine = null;
    }
}
