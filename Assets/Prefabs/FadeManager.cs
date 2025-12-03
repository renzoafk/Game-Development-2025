using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private bool automaticallyFadeInONSceneLoad;
    private Coroutine fadeCoroutine;
    [SerializeField] private float automaticFadeStartAlpha;
    [SerializeField] private float automaticFadeEndAlpha;
    [SerializeField] private float automaticFadeDuration;
    [SerializeField] private float automaticFadeBeforeFade;

    private void Awake()
    {
        if (automaticallyFadeInONSceneLoad)
        {
            DoFade(automaticFadeStartAlpha, automaticFadeEndAlpha, automaticFadeDuration, automaticFadeBeforeFade);
        }
        
    }

    public void DoFade(float startAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(AnimateFade(startAlpha, endAlpha, duration, delayBeforeFade));
    }
    private IEnumerator AnimateFade (float startAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        fadeImage.enabled = true;
        CanvasGroup.alpha = startAlpha;
        yield return null;
        yield return new WaitForSeconds(delayBeforeFade);
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float fadePercentage = timeElapsed / duration;
            fadePercentage = Mathf.Clamp01(fadePercentage);
            CanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, fadePercentage);
            yield return null;
        }
        CanvasGroup.alpha = endAlpha;
        if (endAlpha <= 0)
        {
            fadeImage.enabled = false;
        }
    }
}
