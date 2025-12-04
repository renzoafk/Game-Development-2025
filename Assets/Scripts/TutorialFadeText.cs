using UnityEngine;
using TMPro;

public class TutorialFadeText : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private TextMeshProUGUI tmp;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    private bool playerInside = false;
    private bool fadingIn = false;
    private bool fadingOut = false;
    private float alpha = 0f;

    void Awake()
    {
        if (tmp == null)
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        // Start invisible
        alpha = 0f;
        SetAlpha(alpha);
        gameObject.SetActive(false);
    }

    void Update()
    {
        // Fade in
        if (fadingIn)
        {
            alpha += Time.deltaTime / Mathf.Max(0.0001f, fadeInTime);
            if (alpha >= 1f)
            {
                alpha = 1f;
                fadingIn = false;
            }
            SetAlpha(alpha);
        }

        // Fade out
        if (fadingOut)
        {
            alpha -= Time.deltaTime / Mathf.Max(0.0001f, fadeOutTime);
            if (alpha <= 0f)
            {
                alpha = 0f;
                fadingOut = false;
                SetAlpha(alpha);
                gameObject.SetActive(false);
                return;
            }

            SetAlpha(alpha);
        }
    }

    public void StartFadeOut()
    {
        fadingOut = true;
        fadingIn = false;
    }

    private void StartFadeIn()
    {
        gameObject.SetActive(true);
        fadingIn = true;
        fadingOut = false;
        alpha = 0f;
        SetAlpha(alpha);
    }

    private void SetAlpha(float a)
    {
        a = Mathf.Clamp01(a);
        if (tmp == null) return;

        Color c = tmp.color;
        c.a = a;
        tmp.color = c;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInside = true;
        Debug.Log("Player entered JUMP tutorial trigger");
        StartFadeIn();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInside = false;
        Debug.Log("Player left JUMP tutorial trigger");
        StartFadeOut();
    }
}
