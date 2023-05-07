using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector2 targetPosition;
    private Vector2 playerPosition;
    private Vector2 restPosition;
    private float angle;
    private PlayerMovement playerMovement;
    private EnemyMovement enemyMovement;
    private EnemyAlarm alarm;

    void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        alarm = FindObjectOfType<EnemyAlarm>();
    }
    // Update is called once per frame
    void Update()
    {
        // Get the position of the player in world coordinates
        playerPosition = playerMovement.GetPosition();
        playerPosition.y += 20f;

        if (enemyMovement.GetEnemySpeedX() != 0)
        {
            restPosition = transform.position;
            restPosition.y -= 5f;
            restPosition.x += 5f * enemyMovement.GetEnemySpeedX();
        }
        else
        {
            restPosition = transform.position;
            restPosition.y -= 5f;
            restPosition.x -= 5f;
        }

        if (alarm.IsON)
            targetPosition = playerPosition;
        else
            targetPosition = restPosition;

        // Calculate the angle between the arm and the target
        Vector2 direction = targetPosition - (Vector2)transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Take the original rotation of the arm
        Quaternion originalRotation = transform.rotation;
        // If the character is pointing right, rotate the arm normally
        if (PointingRight())
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, angle);
        // If the character is pointing left, rotate the arm 180 degrees and negate the angle
        else if (!PointingRight())
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, -angle + 180);
    }

    // Returns true if the character is pointing right
    public bool PointingRight()
    {
        if (angle > 0 && angle < 90 || angle < 0 && angle > -90)
            return true;
        else
            return false;
    }
}
