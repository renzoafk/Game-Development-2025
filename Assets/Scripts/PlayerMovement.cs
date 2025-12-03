using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 9.5f;   // a bit higher so we beat the stronger gravity

    [Header("Variable Jump")]
    [SerializeField] private float jumpCutMultiplier = 0.65f;

    [Header("Better Gravity")]
    [SerializeField] private float fallGravityMultiplier = 1.6f;    // more pull when falling
    [SerializeField] private float lowJumpGravityMultiplier = 1.2f; // small extra pull if we let go early

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);  // width, height of box
    [SerializeField] private LayerMask groundLayer;

    [HideInInspector] public float originalMoveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool IsGrounded;
    private bool jumpRequested;
    private bool jumpHeld;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Animator animator;

    private Vector2 Velocity
    {
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        originalMoveSpeed = moveSpeed;

        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
        }
        else
        {
            Debug.LogError("PlayerInput component not found on this object!");
        }
    }

    void OnEnable()
    {
        if (jumpAction != null)
        {
            jumpAction.started += OnJumpStarted;
            jumpAction.canceled += OnJumpCanceled;
        }
    }

    void OnDisable()
    {
        if (jumpAction != null)
        {
            jumpAction.started -= OnJumpStarted;
            jumpAction.canceled -= OnJumpCanceled;
        }
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        if (IsGrounded)
            jumpRequested = true;

        jumpHeld = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        jumpHeld = false;

        // cut the jump a bit if we released while going up
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
    }

    void Update()
    {
        if (moveAction != null)
            moveInput = moveAction.ReadValue<Vector2>();

        // ground check using a box, wider than a circle so edges feel better
        Vector2 checkPos = groundCheck != null ? (Vector2)groundCheck.position : (Vector2)transform.position;
        IsGrounded = Physics2D.OverlapBox(checkPos, groundCheckSize, 0f, groundLayer);

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
            animator.SetBool("IsGrounded", IsGrounded);
            animator.SetFloat("verticalSpeed", rb.linearVelocity.y);
        }

        // flip player sprite based on movement
        if (moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveInput.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        // horizontal
        Velocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // jump
        if (jumpRequested && IsGrounded)
        {
            Velocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        jumpRequested = false;

        // extra gravity controls
        if (rb.linearVelocity.y < 0f)
        {
            // falling, pull harder
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0f && !jumpHeld)
        {
            // going up but we let go, pull a bit harder to end the jump sooner
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpGravityMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 checkPos = groundCheck != null ? (Vector2)groundCheck.position : (Vector2)transform.position;
        Gizmos.DrawWireCube(checkPos, new Vector3(groundCheckSize.x, groundCheckSize.y, 0f));
    }

    internal int GetFacingDirection()
    {
        throw new NotImplementedException();
    }
}
