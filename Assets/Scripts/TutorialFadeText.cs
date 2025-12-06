using UnityEngine;
using TMPro;

public class TutorialFadeText : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI tmp;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInTime = 0.4f;
    [SerializeField] private float fadeOutTime = 0.6f;
    [SerializeField] private float delayAfterComplete = 0.15f;

    [Header("Input that completes this tutorial")]
    [SerializeField] private KeyCode[] completionKeys;

    private enum State
    {
        Hidden,
        FadingIn,
        WaitingForInput,
        DelayAfterInput,
        FadingOut,
        Done
    }

    private State state = State.Hidden;
    private float alpha = 0f;
    private float timer = 0f;

    private void Awake()
    {
        if (tmp == null)
            tmp = GetComponent<TextMeshProUGUI>();

        // start invisible but active
        alpha = 0f;
        SetAlpha(alpha);
        gameObject.SetActive(true);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Hidden:
            case State.Done:
                // do nothing until Show() is called
                return;

            case State.FadingIn:
                timer += Time.deltaTime;
                alpha = Mathf.Clamp01(timer / Mathf.Max(0.0001f, fadeInTime));
                SetAlpha(alpha);

                if (alpha >= 1f)
                {
                    state = State.WaitingForInput;
                    timer = 0f;
                }
                break;

            case State.WaitingForInput:
                if (WasCompletionKeyPressed())
                {
                    state = State.DelayAfterInput;
                    timer = 0f;
                }
                break;

            case State.DelayAfterInput:
                timer += Time.deltaTime;
                if (timer >= delayAfterComplete)
                {
                    state = State.FadingOut;
                    timer = 0f;
                }
                break;

            case State.FadingOut:
                timer += Time.deltaTime;
                float t = timer / Mathf.Max(0.0001f, fadeOutTime);
                alpha = Mathf.Clamp01(1f - t);
                SetAlpha(alpha);

                if (alpha <= 0f)
                {
                    alpha = 0f;
                    SetAlpha(alpha);
                    gameObject.SetActive(false);   // hide completely
                    state = State.Done;
                }
                break;
        }
    }

    private bool WasCompletionKeyPressed()
    {
        if (completionKeys == null) return false;

        foreach (KeyCode key in completionKeys)
        {
            if (Input.GetKeyDown(key))
                return true;
        }
        return false;
    }

    private void SetAlpha(float a)
    {
        if (tmp == null) return;

        a = Mathf.Clamp01(a);
        Color c = tmp.color;
        c.a = a;
        tmp.color = c;
    }

    /// <summary>
    /// Called by a trigger script when the player enters that area.
    /// </summary>
    public void Show()
    {
        if (state != State.Hidden) return;   // only first time

        gameObject.SetActive(true);
        timer = 0f;
        alpha = 0f;
        SetAlpha(alpha);
        state = State.FadingIn;
    }
}
