using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    public bool canAttack = true;    // <<------ ADD THIS

    private Animator anim;
    private PlayerInput playerInput;
    private InputAction attackAction;

    private int comboStep = 0;
    private bool canClick = true;
    public float comboResetTime = 1f;
    private float comboTimer = 0f;

    [Header("Melee Hitbox")]
    [SerializeField] private GameObject meleeHitbox;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
            attackAction = playerInput.actions["Attack"];
        else
        {
            attackAction = new InputAction("Attack", InputActionType.Button, "<Mouse>/leftButton");
            attackAction.Enable();
        }
    }

    void Update()
    {
        if (!canAttack)                      // <<------ ADD THIS
            return;

        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
                ResetCombo();
        }

        if (attackAction != null && attackAction.triggered && canClick)
        {
            TriggerNextAttack();
            Debug.Log("CLICK!");
        }
    }

    void TriggerNextAttack()
    {
        canClick = false;
        comboTimer = 0f;

        comboStep++;
        if (comboStep > 3) comboStep = 3;

        anim.SetInteger("attackIndex", comboStep);
        anim.SetTrigger("Attack");
    }

    public void AllowNextClick()
    {
        canClick = true;
    }

    void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
        anim.SetInteger("attackIndex", 0);
        canClick = true;
    }

    void OnDisable()
    {
        if (attackAction != null)
            attackAction.Disable();
    }

    public void EnableHitbox()
    {
        if (meleeHitbox != null)
        {
            MeleeHitbox hitboxScript = meleeHitbox.GetComponent<MeleeHitbox>();
            if (hitboxScript != null)
                hitboxScript.EnableHitbox();
            else
                Debug.LogError("MeleeHitbox script not found on meleeHitbox GameObject!");
        }
    }

    public void DisableHitbox()
    {
        if (meleeHitbox != null)
        {
            MeleeHitbox hitboxScript = meleeHitbox.GetComponent<MeleeHitbox>();
            if (hitboxScript != null)
                hitboxScript.DisableHitbox();
            else
                Debug.LogError("MeleeHitbox script not found on meleeHitbox GameObject!");
        }
    }
}
