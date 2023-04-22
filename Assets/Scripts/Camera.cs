using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed;

    private Vector3 velocity = Vector3.zero;

    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = target.GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        // If the  position from the target 
        Vector3 desiredPosition = target.position + offset;

        // Negate the offset.x value if the player is moving left
        if (playerMovement.speedX < 0 && transform.position.x > target.position.x)
        {
            offset.x = -Mathf.Abs(offset.x);
        }
        // Negate the offset.x value if the player is moving right
        else if (playerMovement.speedX > 0 && transform.position.x < target.position.x)
        {
            offset.x = Mathf.Abs(offset.x);
        }

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
