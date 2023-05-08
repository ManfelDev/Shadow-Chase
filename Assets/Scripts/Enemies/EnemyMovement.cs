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

    private Rigidbody2D  rb;
    private Animator     animator;
    private bool         onGround = false;
    private bool         isBackwards = false;
    private float        speedX;
    private string       currentState;
    private EnemyAlarm   alarm;
    private EnemyManager enemyManager;
    private FollowPlayer followPlayer;
    private Vector2      playerPosition;
    private Vector2      selfPosition;
    private Vector2      limit1Position;
    private Vector2      limit2Position;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        alarm = FindObjectOfType<EnemyAlarm>();
        followPlayer = GetComponentInChildren<FollowPlayer>();
        enemyManager = GetComponent<EnemyManager>();

        selfPosition = transform.position;
        limit1Position.x = (selfPosition.x + walkLimits);
        limit2Position.x = (selfPosition.x - walkLimits);

        if (canMove)
            speedX = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyManager.GetDead())
        {
            Vector2 deadVelocity;
            deadVelocity.x = 0;
            deadVelocity.y = 0;
            rb.velocity = deadVelocity;

            ChangeAnimationState("Dead");
        }
        else
        {
            DetectGround();
            playerPosition = GameObject.FindWithTag("Player").transform.position;
            selfPosition = transform.position;

            groundCollider.enabled = onGround;
            airCollider.enabled = !onGround;

            // Update character horizontal velocity
            Vector2 currentVelocity = rb.velocity;

            // Inverts the speed when it walks past a limit
            if (canMove)
            {
                if ((selfPosition.x <= limit1Position.x && walkLimits < 0) || (selfPosition.x <= limit2Position.x && walkLimits > 0))
                    speedX = 1;

                else if ((selfPosition.x >= limit1Position.x && walkLimits > 0) || (selfPosition.x >= limit2Position.x && walkLimits < 0))
                    speedX = -1;

                ChangeAnimationState("Walk");
            }
            // Stops the enemy if they're not supposed to move
            else
                speedX = 0;

            // Changes the animation to idle whenever the enemy is not moving
            if (speedX == 0)
            {
                ChangeAnimationState("Idle");
            }

            // Calculate movement
            currentVelocity.x = speedX * moveSpeed;

            // Check if the enemy is grounded and alarmed, if the player is higher than itself and if it can jump
            if (onGround && alarm.IsON && (((selfPosition.y + 20f) < playerPosition.y) && canJump))
            {
                // Calculate the velocity needed to achieve the desired jump height
                currentVelocity.y = Mathf.Sqrt(2f * rb.gravityScale * jumpForce * rb.mass);
                // Apply gravity
                currentVelocity.y -= rb.gravityScale * Time.deltaTime;

                ChangeAnimationState("Jump");
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
            if ((speedX >= 0 && !followPlayer.PointingRight() ||
                speedX <= 0 && followPlayer.PointingRight()))
                isBackwards = true;
            else
                isBackwards = false;

            // Change visuals
            animator.SetFloat("Speed", Mathf.Abs(speedX));
            animator.SetBool("onGround", onGround);
            animator.SetBool("isBackwards", isBackwards);
        }
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

    public bool CheckIfCanMove()
    {
        return canMove;
    }

    public bool CheckIfCanJump()
    {
        return canJump;
    }

    //Draws indicators on the editor to check the walk limits of the enemy
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

// Changes the character's animation
    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }
}