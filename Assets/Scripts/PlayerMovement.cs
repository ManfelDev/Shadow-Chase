using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 70.0f;
    [SerializeField] public float jumpForce = 90.0f;
    private bool isGrounded = true;

    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player horizontally
        MoveHorizontally();

        // Check if the player is grounded and the jump button is pressed
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // Move the player horizontally
    private void MoveHorizontally()
    {
        Vector2 currentVelocity = rb.velocity;
        // Set the x velocity to the horizontal input times the move speed
        currentVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = currentVelocity;
        animator.SetFloat("Speed", Mathf.Abs(currentVelocity.x));

        // Flip the player sprite if needed
        if (currentVelocity.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (currentVelocity.x > 0) transform.rotation = Quaternion.identity;
    }

    // Make the player jump
    private void Jump()
    {
        Vector2 currentVelocity = rb.velocity;
        // Calculate the velocity needed to achieve the desired jump height
        currentVelocity.y = Mathf.Sqrt(2f * rb.gravityScale * jumpForce * rb.mass);
        // Apply gravity
        currentVelocity.y -= rb.gravityScale * Time.deltaTime;
        rb.velocity = currentVelocity;
        animator.SetBool("IsJumping", true);
    }

    // When the player lands on the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is colliding with the ground
        if (collision.gameObject.tag == "Platforms")
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }

    // When the player leaves the ground
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player is colliding with the ground
        if (collision.gameObject.tag == "Platforms")
        {
            isGrounded = false;
        }
    }
}
