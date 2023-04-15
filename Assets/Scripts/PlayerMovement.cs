using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Create a variable to hold the move speed
    [SerializeField] private float moveSpeed = 100.0f;
    // Jump float
    [SerializeField] private float jumpForce = 90.0f;

    // Create a variable to hold the rigidbody component
    private Rigidbody2D rb;

    // Create a variable to hold the ground check
    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Put the current velocity into a variable
        Vector2 currentVelocity = rb.velocity;

        // Set the x velocity to the horizontal input times the move speed
        currentVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Player jump with jump button with rb gravity
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            currentVelocity.y = jumpForce;
            isGrounded = false;
        }

        // Apply gravity
        currentVelocity.y -= rb.gravityScale * Time.deltaTime;

        // Set the rigidbody's velocity to the current velocity
        rb.velocity = currentVelocity;
    }

    // When the player lands on the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is colliding with the ground
        if (collision.gameObject.tag == "Platforms")
        {
            isGrounded = true;
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
