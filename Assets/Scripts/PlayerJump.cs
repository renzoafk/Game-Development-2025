using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 6;
    public Animator anim;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Update yVelocity for falling animations
        if (anim != null && rb != null)
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
        }

        // TEMPORARY: Remove ground check to test jumping
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            if (anim != null)
            {
                anim.SetTrigger("Jumping");
            }
        }
    }

    private void Jump()
    {
        // Make sure rb is assigned
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // Or use: rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody2D not assigned!");
        }
    }
}