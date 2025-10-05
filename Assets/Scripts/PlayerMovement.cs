using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float speed = 5;
    private int facingDirection = 1;

    public Rigidbody2D rb;
    public Animator anim;


    // Update is called once per frame
    void FixedUpdate()
    {

        float Horizontal = Input.GetAxis("Horizontal");
        //float Vertical = Input.GetAxis("Vertical");
        
        if (Horizontal > 0 && transform.localScale.x < 0 ||
            Horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.SetFloat("Horizontal", Mathf.Abs(Horizontal));
        //anim.SetFloat("Vertical", Mathf.Abs(Vertical));


        rb.linearVelocity = new Vector2(Horizontal * speed, rb.linearVelocity.y);


    }


    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    

}
