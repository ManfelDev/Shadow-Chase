using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float      moveSpeed = 80.0f;
    [SerializeField] private float      jumpForce = 5.0f;
    [SerializeField] private Transform  groundDetector;
    [SerializeField] private float      groundDetectorRadius = 2.0f;
    [SerializeField] private float      groundDetectorExtraRadius = 6.0f;
    [SerializeField] private LayerMask  groundMask;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private Collider2D airCollider;

    private Rigidbody2D rb;
    private Animator    animator;
    private bool        onGround = false;
    private float       speedX;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectGround();

        groundCollider.enabled = onGround;
        airCollider.enabled = !onGround;

        // Update character horizontal velocity
        Vector2 currentVelocity = rb.velocity;
        speedX = Input.GetAxis("Horizontal");
        currentVelocity.x = speedX * moveSpeed;

        // Check if the player is grounded and the jump button is pressed
        if (onGround && Input.GetButtonDown("Jump"))
        {
            // Calculate the velocity needed to achieve the desired jump height
            currentVelocity.y = Mathf.Sqrt(2f * rb.gravityScale * jumpForce * rb.mass);
            // Apply gravity
            currentVelocity.y -= rb.gravityScale * Time.deltaTime;
        }

        // Apply movement
        rb.velocity = currentVelocity;

        // Flip the character if needed
        if (speedX < 0 && transform.right.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (speedX > 0 && transform.right.x < 0)
        {
            transform.rotation = Quaternion.identity;
        }

        // Change visuals
        animator.SetFloat("Speed", Mathf.Abs(speedX));
        animator.SetBool("onGround", onGround);
    }

    void DetectGround()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundDetector.position, groundDetectorRadius, groundMask);
        if (collider != null) onGround = true;
        else
        {
            collider = Physics2D.OverlapCircle(groundDetector.position - Vector3.right * groundDetectorExtraRadius, groundDetectorRadius, groundMask);
            if (collider != null) onGround = true;
            else
            {
                collider = Physics2D.OverlapCircle(groundDetector.position + Vector3.right * groundDetectorExtraRadius, groundDetectorRadius, groundMask);
                if (collider != null) onGround = true;
                else onGround = false;
            }
        }
    }

    public int GetPlayerSpeedX()
    {
        return (int)speedX;
    }
}