using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private bool       canMove = false;
    [SerializeField] private float      moveSpeed = 80.0f;
    [SerializeField] private int        walkLimits = 10;
    [SerializeField] private bool       canJump = false;
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
    private bool        isBackwards = false;
    private float       speedX;
    private FollowPlayer followPlayer;
    private Vector2 playerPosition;
    private Vector2 selfPosition;
    private Vector2 limit1Position;
    private Vector2 limit2Position;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        followPlayer = GetComponentInChildren<FollowPlayer>();
        selfPosition = transform.position;
        if (canMove)
        {
            speedX = 1;
            limit1Position.x = (selfPosition.x + walkLimits);
            limit2Position.x = (selfPosition.x - walkLimits);
        }

    }

    // Update is called once per frame
    void Update()
    {
        DetectGround();
        playerPosition = GameObject.FindWithTag("Player").transform.position;
        selfPosition = transform.position;

        groundCollider.enabled = onGround;
        airCollider.enabled = !onGround;

        // Update character horizontal velocity
        Vector2 currentVelocity = rb.velocity;
        if (canMove)
        {
            if ((selfPosition.x <= limit1Position.x && walkLimits < 0) || (selfPosition.x <= limit2Position.x && walkLimits > 0))
                speedX = 1;

            else if ((selfPosition.x >= limit1Position.x && walkLimits > 0) || (selfPosition.x >= limit2Position.x && walkLimits < 0))
                speedX = -1;

        }
        currentVelocity.x = speedX * moveSpeed;

        // Check if the enemy is grounded and the jump button is pressed
        if (onGround && (((selfPosition.y + 20f) < playerPosition.y) && canJump))
        {
            // Calculate the velocity needed to achieve the desired jump height
            currentVelocity.y = Mathf.Sqrt(2f * rb.gravityScale * jumpForce * rb.mass);
            // Apply gravity
            currentVelocity.y -= rb.gravityScale * Time.deltaTime;
        }

        // Apply movement
        rb.velocity = currentVelocity;

        // If the enemy is pointing right does nothing
        if (followPlayer.PointingRight())
            transform.rotation = Quaternion.identity;
        // If the enemy is pointing left, rotate everything 180 degrees
        else if (!followPlayer.PointingRight())
            transform.rotation = Quaternion.Euler(0, 180, 0);

        // Check if the enemy is moving backwards
        if (speedX >= 0 && !followPlayer.PointingRight() ||
            speedX <= 0 && followPlayer.PointingRight())
            isBackwards = true;
        else
            isBackwards = false;

        // Change visuals
        animator.SetFloat("Speed", Mathf.Abs(speedX));
        animator.SetBool("onGround", onGround);
        animator.SetBool("isBackwards", isBackwards);
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

    public int GetEnemySpeedX()
    {
        return (int)speedX;
    }

    private void OnDrawGizmos()
    {
        if(groundDetector != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + transform.right * walkLimits, 3);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position - transform.right * walkLimits, 3);
        }
    }
}