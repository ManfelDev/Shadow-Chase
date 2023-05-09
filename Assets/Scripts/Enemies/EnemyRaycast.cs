using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaycast : MonoBehaviour
{
    [SerializeField] private float      detectionRadius = 80.0f;

    private EnemyAlarm   alarm;
    private GameObject   player;
    private Vector2      playerPosition;
    private Vector2      selfPosition;
    private Vector2      enemyToPlayer;
    private float        playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        alarm = FindObjectOfType<EnemyAlarm>();

        // Updates its own position to its eyes
        selfPosition = transform.position;
        selfPosition.y += 30f;

        // Identifies the player
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Update the player's position to the center of their body
        playerPosition = player.transform.position;
        playerPosition.y += 20f;

        // Calculate if the player is within detection range
        enemyToPlayer = selfPosition - playerPosition;
        playerDistance = Mathf.Sqrt((enemyToPlayer.x * enemyToPlayer.x) + (enemyToPlayer.y * enemyToPlayer.y));

        if (playerDistance <= detectionRadius)
        {
            // Send a raycast from the enemy towards the player
            RaycastHit2D raycast = Physics2D.Raycast(selfPosition, selfPosition - playerPosition);

            // If the raycast detects the player, begins the alarm countdown
            if (raycast.collider.CompareTag("Player"))
            {
                alarm.Trigger();
            }
        }
    }
}
