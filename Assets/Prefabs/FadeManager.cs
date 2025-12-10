using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [Header("Fade Elements")]
    [SerializeField] private Image fadeImage;        // full-screen black Image
    [SerializeField] private CanvasGroup fadeGroup;  // CanvasGroup on the same object

    [Header("Auto Fade-In")]
    [SerializeField] private bool autoFadeInOnSceneLoad = true;
    [SerializeField] private float autoStartAlpha = 1f;
    [SerializeField] private float autoEndAlpha = 0f;
    [SerializeField] private float autoDuration = 1f;
    [SerializeField] private float autoDelay = 0f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Try to auto-grab references if missing
        if (fadeImage == null)
            fadeImage = GetComponentInChildren<Image>(true);

        if (fadeGroup == null && fadeImage != null)
            fadeGroup = fadeImage.GetComponent<CanvasGroup>();

        // Make sure fade image is enabled if we’re going to use it
        if (fadeImage != null)
            fadeImage.enabled = true;

        if (fadeGroup != null)
            fadeGroup.alpha = autoStartAlpha;

        // Optional automatic fade in
        if (autoFadeInOnSceneLoad)
        {
            DoFade(autoStartAlpha, autoEndAlpha, autoDuration, autoDelay);
        }
        else
        {
            // If we start fully transparent, hide the image
            if (fadeGroup != null && fadeGroup.alpha <= 0f && fadeImage != null)
                fadeImage.enabled = false;
        }
    }

    // One-shot fade you can call from other scripts
    public void DoFade(float startAlpha, float endAlpha, float duration, float delayBeforeFade = 0f)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(startAlpha, endAlpha, duration, delayBeforeFade));
    }

    // Coroutine helpers used by other scripts (CutsceneToBossFight etc.) :contentReference[oaicite:1]{index=1}
    public IEnumerator FadeToBlack(float duration, float delayBeforeFade = 0f)
    {
        float startAlpha = fadeGroup != null ? fadeGroup.alpha : 0f;
        yield return FadeRoutine(startAlpha, 1f, duration, delayBeforeFade);
    }

    public IEnumerator FadeFromBlack(float duration, float delayBeforeFade = 0f)
    {
        float startAlpha = fadeGroup != null ? fadeGroup.alpha : 1f;
        yield return FadeRoutine(startAlpha, 0f, duration, delayBeforeFade);
    }

    // Shared fade logic
    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        if (fadeImage != null)
            fadeImage.enabled = true;

        if (fadeGroup != null)
            fadeGroup.alpha = startAlpha;

        // Optional delay
        if (delayBeforeFade > 0f)
            yield return new WaitForSeconds(delayBeforeFade);

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);

            if (fadeGroup != null)
                fadeGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        if (fadeGroup != null)
            fadeGroup.alpha = endAlpha;

        // If fully transparent at the end, we can disable the image
        if (endAlpha <= 0f && fadeImage != null)
            fadeImage.enabled = false;

        fadeCoroutine = null;
    }
}
