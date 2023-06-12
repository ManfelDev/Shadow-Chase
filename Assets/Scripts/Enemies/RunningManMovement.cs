using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningManMovement : MonoBehaviour
{
    [SerializeField] private float      moveSpeed = 65.0f;
    [SerializeField] private float      jumpForce = 5.0f;
    [SerializeField] private Transform  groundDetector;
    [SerializeField] private float      groundDetectorRadius = 2.0f;
    [SerializeField] private float      groundDetectorExtraRadius = 6.0f;
    [SerializeField] private LayerMask  groundMask;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private Collider2D airCollider;

    private Rigidbody2D       rb;
    private Animator          animator;
    private bool              onGround = false;
    private float             speedX;
    private float             oldSpeedX;
    private string            currentState;
    private EnemyAlarm        alarm;
    private RunningManRaycast runningManRaycast;
    private Vector2           playerPosition;
    private Vector2           selfPosition;
    private bool              jumpPoint;
    private bool              alarmJumpPoint;
    private bool              turnPoint;
    private bool              pausePoint;
    private bool              isPaused;
    private float             lastJump;
    private float             lastPause;
    private float             lastTurn;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        alarm = FindObjectOfType<EnemyAlarm>();
        runningManRaycast = GetComponent<RunningManRaycast>();
        Physics2D.IgnoreLayerCollision(8, 7, true);

        speedX = 1;
        oldSpeedX = speedX;

        // Initializes all 'last' variables to 100 seconds before the start of the game
        lastJump = -100f;
        lastPause = -100f;
        lastTurn = -100f;

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

        if (runningManRaycast.GetCountdown()<= 2.7f && !alarm.IsON)
            speedX = 0;

        else if (!isPaused)
            speedX = oldSpeedX;
        

        // Inverts the speed when it reaches a turning point at least 2 seconds after the last turn
        if (turnPoint)
            {
                speedX *= -1;
                oldSpeedX = speedX;
                lastTurn = Time.time;
                turnPoint = false;
            }
            
        // Stops the enemy for 5 seconds if they reach a pause point after 10 seconds of the previous one
        if (pausePoint)
        {
            if (Time.time >= lastPause + 10f)
            {
                lastPause = Time.time;
                oldSpeedX = speedX;
                speedX = 0;
                isPaused = true;
            }

            else if (!isPaused)
                pausePoint = false;


            if ((isPaused && Time.time >= lastPause + 5f  && runningManRaycast.GetCountdown()>= 3f) || alarm.IsON)
            {
                speedX = oldSpeedX;
                isPaused = false;
                pausePoint = false;
                ChangeAnimationState("Walk");
            }

        }

        // Changes the animation to idle whenever the enemy is not moving
        if (speedX == 0)
        {
            ChangeAnimationState("Idle");
        }

        // Calculate movement
        if (alarm.IsON)
            speedX *= 1.5f;

        currentVelocity.x = speedX * moveSpeed;

        // Check if the enemy is grounded and if it reached a jumping point
        if ((onGround && jumpPoint && !alarm.IsON) || (onGround && alarmJumpPoint && alarm.IsON))
            {
                // Calculate the velocity needed to achieve the desired jump height
                currentVelocity.y = Mathf.Sqrt(2f * rb.gravityScale * jumpForce * rb.mass);
                // Apply gravity
                currentVelocity.y -= rb.gravityScale * Time.deltaTime;

                jumpPoint = false;
                alarmJumpPoint = false;

                lastJump = Time.time;

                ChangeAnimationState("Jump");
            }

        // Apply movement
        rb.velocity = currentVelocity;

        // If the enemy is suspicious, look towards the player
        if (runningManRaycast.GetCountdown() < 3 && !alarm.IsON)
            if (runningManRaycast.GetDirection() > 0)
                transform.rotation = Quaternion.identity;
            
            else transform.rotation = Quaternion.Euler(0, 180, 0);

        // If the enemy is pointing right does nothing
        else if (speedX > 0)
            transform.rotation = Quaternion.identity;

        // If the enemy is pointing left, rotate everything 180 degrees
        else if (speedX <= 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);

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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "JumpTrigger" && Time.time >= lastJump + 0.2f)
            jumpPoint = true;

        if (col.gameObject.tag == "AlarmJumpTrigger" && Time.time >= lastJump + 0.2f)
            alarmJumpPoint = true;

        if (col.gameObject.tag == "PauseTrigger" && !alarm.IsON)
            pausePoint = true;

        if (col.gameObject.tag == "TurnTrigger" && Time.time >= lastTurn + 0.2f)
            turnPoint = true;
    }

    public int GetEnemySpeedX()
    {
        return (int)speedX;
    }

// Changes the character's animation
    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }

    //Draws indicators on the editor to check the ground detector
    private void OnDrawGizmos()
    {
        if (groundDetector == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundDetector.position, groundDetectorRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundDetector.position - Vector3.right * groundDetectorExtraRadius, groundDetectorRadius);
        Gizmos.DrawSphere(groundDetector.position + Vector3.right * groundDetectorExtraRadius, groundDetectorRadius);
    }
}
