using UnityEngine;
using UnityEngine.InputSystem;   // ok if you are using the new Input System

public class Attack : MonoBehaviour
{
    [Header("General")]
    public bool canAttack = true;
    public float comboResetTime = 1f;

    [Header("Hitbox")]
    [SerializeField] private GameObject meleeHitbox;   // your hitbox object

    private Animator anim;

    // combo state
    private int comboStep = 0;
    private bool canClick = true;
    private float comboTimer = 0f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("[Attack] No Animator found on this GameObject!");
    }

    void Update()
    {
        if (!canAttack) return;

        // combo timeout
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
                ResetCombo();
        }

        // ✅ Always listen for left click, regardless of PlayerInput setup
        bool leftClick =
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) // new Input System
            || Input.GetMouseButtonDown(0);                                         // old Input Manager fallback

        if (leftClick && canClick)
        {
            Debug.Log("[Attack] Left click detected, starting attack");
            TriggerNextAttack();
        }
    }

    private void TriggerNextAttack()
    {
        canClick = false;
        comboTimer = 0f;

        comboStep++;
        if (comboStep > 3) comboStep = 3;   // max 3-hit combo, adjust if you want

        if (anim != null)
        {
            anim.SetInteger("attackIndex", comboStep);  // your animator int
            anim.SetTrigger("Attack");                  // your animator trigger
        }
    }

    // call this from an Animation Event at the point where the next click is allowed
    public void AllowNextClick()
    {
        canClick = true;
    }

    private void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
        canClick = true;

        if (anim != null)
            anim.SetInteger("attackIndex", 0);
    }

    // hitbox control, also usually called by animation events
    public void EnableHitbox()
    {
        if (meleeHitbox != null)
        {
            var hb = meleeHitbox.GetComponent<MeleeHitbox>();
            if (hb != null) hb.EnableHitbox();
        }
    }

    public void DisableHitbox()
    {
        if (meleeHitbox != null)
        {
            var hb = meleeHitbox.GetComponent<MeleeHitbox>();
            if (hb != null) hb.DisableHitbox();
        }
    }
}
