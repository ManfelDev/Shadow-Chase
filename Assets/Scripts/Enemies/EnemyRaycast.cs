using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaycast : MonoBehaviour
{
    [SerializeField] private float      detectionRadius = 100.0f;
    [SerializeField] private float      countdownTimer = 3.0f;
    [SerializeField] private float      countdownTicks = 0.1f;
    [SerializeField] private GameObject playerDetector;
    [SerializeField] private GameObject suspicionIcon;
    [SerializeField] private GameObject detectionIcon;

    private EnemyAlarm    alarm;
    private EnemyMovement enemyMovement;
    private GameObject    player;
    private Vector2       playerPosition;
    private Vector2       enemyToPlayer;
    private FollowPlayer  followPlayer;
    private float         playerDistance;
    private float         countdown;
    private float         lastTick;
    private Vector2       selfPosition;
    private RaycastHit2D  raycast;
    private float         direction;
    private Collider2D[]  playerColliders;
    private bool          slowTrigger;

    // Start is called before the first frame update
    void Start()
    {
        alarm = FindObjectOfType<EnemyAlarm>();
        slowTrigger = false;

        selfPosition = transform.position;
        // Updates its own position to its eyes
        selfPosition.y += 20f;

        // Identifies the player
        player = GameObject.FindWithTag("Player");
        playerColliders = player.GetComponents<Collider2D>();

        // Initializes countdown
        countdown = countdownTimer;

        enemyMovement = GetComponentInParent<EnemyMovement>();
        followPlayer = GameObject.FindObjectOfType<FollowPlayer>();
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

        // Rotates the detection icons to match the player's rotation
        suspicionIcon.transform.localRotation = transform.localRotation;
        detectionIcon.transform.localRotation = transform.localRotation;

        // If slow trigger is active, the alarm will raise continuously
        if (slowTrigger)
        {
            if (Time.time > lastTick + countdownTicks)
                {
                    countdown -= countdownTicks;
                    lastTick = Time.time;
                }
        }

        else if (playerDistance <= detectionRadius)
        {
            // Send a raycast from the enemy's PlayerDetector towards the player
            if (enemyMovement.GetEnemySpeedX() != 0)
                direction = enemyMovement.GetEnemySpeedX();

            else
            {
                if (enemyToPlayer.x >= 0)
                    direction = -1;

                else direction = 1;
            }

            for (float i = 0; i < detectionRadius; i+= 0.1f)
            {
                Vector2 rayPos = playerDetector.transform.position;
                rayPos.x += i * direction;
                raycast = Physics2D.Raycast(rayPos, Vector2.right * new Vector2(direction * detectionRadius, 0f), 0.1f);

                if (CheckRaycastCollision(raycast) == "Ground")
                {
                    Debug.DrawRay(playerDetector.transform.position, Vector2.right * new Vector2(direction * detectionRadius, 0f), Color.blue);
                    break;
                }

                // If the raycast detects the player, lowers the alarm countdown
                else if (CheckRaycastCollision(raycast) == "Player")
                {
                    if (Time.time > lastTick + countdownTicks)
                    {
                        countdown -= countdownTicks;
                        lastTick = Time.time;
                    }
                    Debug.DrawRay(playerDetector.transform.position, Vector2.right * new Vector2(direction * detectionRadius, 0f), Color.green);
                    break;
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

        if (alarm.IsON)
        {
            detectionIcon.SetActive(true);
            suspicionIcon.SetActive(false);
        }

        else if (countdown < countdownTimer)
        {
            detectionIcon.SetActive(false);
            suspicionIcon.SetActive(true);
        }

        else
        {
            detectionIcon.SetActive(false);
            suspicionIcon.SetActive(false);
        }
    }

    // Draw gizmos to visualize the detection radius
    void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(selfPosition, detectionRadius);
    }

    private string CheckRaycastCollision(RaycastHit2D raycast)
    {
        if(raycast.collider != null)
            return raycast.collider.gameObject.tag;
        else return "";
    }

    private Transform GetHighestParentTransform(Transform transform)
    {
        Transform highestParent = transform;
        while (highestParent.parent != null)
        {
            highestParent = highestParent.parent;
        }
        return highestParent;
    }

    public void SlowTrigger()
    {
        slowTrigger = true;
    }

    public float GetCountdown()
    {
        return countdown;
    }

    public float GetDirection()
    {
        return direction;
    }
}
