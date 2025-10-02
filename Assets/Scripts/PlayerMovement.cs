using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private int facingDirection = 1;

    public Rigidbody2D rb;
    public Animator anim;

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0 && facingDirection < 0 ||
            horizontal < 0 && facingDirection > 0)
        {
            Flip();
        }

        anim.SetFloat("Speed", Mathf.Abs(horizontal));
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    void Flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x = facingDirection;
        transform.localScale = scale;
    }
}
