using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class WorldTutorialMessage : MonoBehaviour
{
    [Header("Fade")]
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 0.5f;
    public float delayBeforeFadeOut = 0.1f;

    [Header("When should this message listen?")]
    public bool requirePlayerInside = false;
    public string playerTag = "Player";

    [Header("Keys that dismiss this message")]
    public KeyCode[] keysToDismiss;

    private TextMeshPro tmp;
    private float alpha = 0f;
    private bool fadingOut = false;
    private bool conditionMet = false;
    private bool playerInside = false;

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        SetAlpha(0f); // start invisible, fade in
    }

    void Update()
    {
        // Fade in
        if (!fadingOut && alpha < 1f)
        {
            alpha += Time.deltaTime / Mathf.Max(0.0001f, fadeInTime);
            SetAlpha(alpha);
        }

        // Check keys
        if (!conditionMet)
        {
            if (!requirePlayerInside || playerInside)
            {
                foreach (var key in keysToDismiss)
                {
                    if (Input.GetKeyDown(key))
                    {
                        conditionMet = true;
                        Invoke(nameof(StartFadeOut), delayBeforeFadeOut);
                        break;
                    }
                }
            }
        }

        // Fade out
        if (fadingOut)
        {
            alpha -= Time.deltaTime / Mathf.Max(0.0001f, fadeOutTime);
            SetAlpha(alpha);

            if (alpha <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void StartFadeOut()
    {
        fadingOut = true;
    }

    void SetAlpha(float a)
    {
        a = Mathf.Clamp01(a);
        var c = tmp.color;
        c.a = a;
        tmp.color = c;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            playerInside = true;
        Debug.Log("Player entered JUMP tutorial trigger");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            playerInside = false;
        Debug.Log("Player left JUMP tutorial trigger");
    }

}
