using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaycast : MonoBehaviour
{
    [SerializeField] private float      detectionRadius = 80.0f;
    [SerializeField] private float      countdownTimer = 3.0f;
    [SerializeField] private float      countdownTicks = 0.1f;
    [SerializeField] private GameObject playerDetector;

    private EnemyAlarm    alarm;
    private EnemyMovement enemyMovement;
    private GameObject    player;
    private Vector2       playerPosition;
    private Vector2       enemyToPlayer;
    private float         playerDistance;
    private float         countdown;
    private float         lastTick;
    private Vector2       selfPosition;
    private RaycastHit2D  raycast;
    private float         direction;
    private Collider2D[] playerColliders;

    // Start is called before the first frame update
    void Start()
    {
        alarm = FindObjectOfType<EnemyAlarm>();

        selfPosition = transform.position;
        // Updates its own position to its eyes
        selfPosition.y += 20f;

        // Identifies the player
        player = GameObject.FindWithTag("Player");
        playerColliders = player.GetComponents<Collider2D>();

        // Initializes countdown
        countdown = countdownTimer;

        enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the player's position to the center of their body
        playerPosition = player.transform.position;
        playerPosition.y += 20f;

        // Update the enemy's position to the center of their eyes
        selfPosition = transform.position;
        selfPosition.y += 30f;

        // Calculate if the player is within detection range
        enemyToPlayer = selfPosition - playerPosition;
        playerDistance = Mathf.Sqrt((enemyToPlayer.x * enemyToPlayer.x) + (enemyToPlayer.y * enemyToPlayer.y));

        //if (playerDistance <= detectionRadius)
        if (true)
        {
            // Send a raycast from the enemy's PlayerDetector towards the player
            if (enemyMovement.GetEnemySpeedX() != 0)
                direction = enemyMovement.GetEnemySpeedX();
            else
                direction = -1;

            for (float i = 0; i < detectionRadius; i+= 0.1f)
            {
                Vector2 rayPos = playerDetector.transform.position;
                rayPos.x += i * direction;
                raycast = Physics2D.Raycast(rayPos, Vector2.right * new Vector2(direction * detectionRadius, 0f), 0.1f);

                // If the raycast detects the player, lowers the alarm countdown
                if (CheckRaycastCollision())
                {
                    if (Time.time > lastTick + countdownTicks)
                    {
                        countdown -= countdownTicks;
                        lastTick = Time.time;
                    }
                    Debug.DrawRay(playerDetector.transform.position, Vector2.right * new Vector2(direction * detectionRadius, 0f), Color.red);
                }

                else
                    Debug.DrawRay(playerDetector.transform.position, Vector2.right * new Vector2(direction * detectionRadius, 0f), Color.red);
            }
        }
        // If the raycast doesn't detect the player, raises the alarm countdown
        else if (countdown < countdownTimer)
        {
            if (Time.time > lastTick + countdownTicks)
            {
                countdown += countdownTicks;
                lastTick = Time.time;
            }

            // The countdown caps at the set limit
            if (countdown > countdownTimer)
                countdown = countdownTimer;
        }
        
        if (countdown <= 0)
            alarm.Trigger();
    }

    // Draw gizmos to visualize the detection radius
    void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(selfPosition, detectionRadius);
    }

    private bool CheckRaycastCollision()
    {
        foreach (Collider2D c in playerColliders)
        {
            if (c == raycast.collider)
                return true;
        }
        return false;
    }
}
