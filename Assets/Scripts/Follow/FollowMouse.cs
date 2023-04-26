using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector2 targetPosition;
    private float   angle;

    // Update is called once per frame
    void Update()
    {
        // Get the position of the mouse cursor in world coordinates
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the angle between the arm and the mouse cursor
        Vector2 direction = targetPosition - (Vector2)transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Take the original rotation of the arm
        Quaternion originalRotation = transform.rotation;
        // If the player is pointing right, rotate the arm normally
        if (PointingRight())
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, angle);
        // If the player is pointing left, rotate the arm 180 degrees and negate the angle
        else if (!PointingRight())
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, -angle + 180);
    }

    // Returns true if the player is pointing right
    public bool PointingRight()
    {
        if (angle > 0 && angle < 90 || angle < 0 && angle > -90)
            return true;
        else
            return false;
    }
}
