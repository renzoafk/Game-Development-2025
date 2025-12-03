using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private Animator anim;
    private PlayerInput playerInput;
    private InputAction attackAction;

    private int comboStep = 0;        // 0 = idle, 1 = attack1, 2 = attack2, 3 = attack3
    private bool canClick = true;     // True only when player can trigger next attack
    public float comboResetTime = 1f; // Reset combo if idle too long
    private float comboTimer = 0f;

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
        // Reset combo if idle too long
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
                ResetCombo();
        }

        // Handle attack input
        if (attackAction != null && attackAction.triggered && canClick)
        {
            TriggerNextAttack();
        }
    }

    void TriggerNextAttack()
    {
        canClick = false;        // Prevent multiple triggers until animation allows
        comboTimer = 0f;

        comboStep++;
        if (comboStep > 3) comboStep = 3;

        anim.SetInteger("attackIndex", comboStep);
        anim.SetTrigger("Attack");
    }

    // Call this at the END of each animation via Animation Event
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
}
