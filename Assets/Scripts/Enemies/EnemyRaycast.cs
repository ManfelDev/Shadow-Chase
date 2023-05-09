using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaycast : MonoBehaviour
{
    [SerializeField] private float      detectionRadius = 80.0f;
    [SerializeField] private float      countdownTimer = 3.0f;
    [SerializeField] private float      countdownTicks = 0.1f;

    private EnemyAlarm   alarm;
    private GameObject   player;
    private Vector2      playerPosition;
    private Vector2      enemyToPlayer;
    private float        playerDistance;
    private float        countdown;
    private float        lastTick;
    private Vector2      selfPosition;

    // Start is called before the first frame update
    void Start()
    {
        alarm = FindObjectOfType<EnemyAlarm>();

        selfPosition = transform.position;
        // Updates its own position to its eyes
        selfPosition.y += 30f;

        // Identifies the player
        player = GameObject.FindWithTag("Player");

        // Initializes countdown
        countdown = countdownTimer;
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

        if (playerDistance <= detectionRadius)
        {
            // Send a raycast from the enemy towards the player
            RaycastHit2D raycast = Physics2D.Raycast(selfPosition, playerPosition - selfPosition);

            // If the raycast detects the player, lowers the alarm countdown
            if (raycast.collider.CompareTag("Player"))
            {
                if (Time.time > lastTick + countdownTicks)
                {
                    countdown -= countdownTicks;
                    lastTick = Time.time;
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

        }

        if (countdown <= 0)
            alarm.Trigger();
        
        Debug.Log(countdown);
    }

    // Draw gizmos to visualize the detection radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(selfPosition, detectionRadius);
        // Draw raycast
        Gizmos.DrawLine(selfPosition, playerPosition - selfPosition);
    }
}
