using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance;

    public float duration = 1f;

    private Image img;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        img = GetComponent<Image>();

        // Make sure we start fully black
        Color c = img.color;
        c.a = 1f;
        img.color = c;
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = img.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / duration);
            c.a = a;
            img.color = c;
            yield return null;
        }

        c.a = 0f;
        img.color = c;
    }

    public IEnumerator FadeOutAndLoad(string sceneName)
    {
        float t = 0f;
        Color c = img.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 1f, t / duration);
            c.a = a;
            img.color = c;
            yield return null;
        }

        c.a = 1f;
        img.color = c;

        SceneManager.LoadScene(sceneName);
    }
}
