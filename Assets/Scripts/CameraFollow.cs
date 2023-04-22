using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] BoxCollider2D cameraBounds;

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed;

    private Vector3 velocity = Vector3.zero;

    private PlayerMovement playerMovement;

    new Camera camera;

    void Awake()
    {
        playerMovement = target.GetComponent<PlayerMovement>();
        camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Negate the offset.x value if the player is moving left
        if (playerMovement.speedX < 0 && transform.position.x > target.position.x)
        {
            offset.x = -Mathf.Abs(offset.x);
        }
        // Set the offset.x value if the player is moving right
        else if (playerMovement.speedX > 0 && transform.position.x < target.position.x)
        {
            offset.x = Mathf.Abs(offset.x);
        }

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Make sure the camera doesn't go outside the bounds
        CheckBounds();
    }

    public void CheckBounds()
    {
        // Return if the camera bounds are not set
        if (cameraBounds == null) 
            return;

        // Get the bounds of the camera and the renderer
        Bounds b = cameraBounds.bounds;

        // Calculate the half height and width of the camera
        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        // Calculate the bounds on the x and y axis
        float xMin = transform.position.x - halfWidth;
        float xMax = transform.position.x + halfWidth;
        float yMin = transform.position.y - halfHeight;
        float yMax = transform.position.y + halfHeight;

        // Get the camera's position
        Vector3 position = transform.position;

        // Check if the camera is outside the bounds on the x-axis
        if (xMin <= b.min.x) 
            position.x = b.min.x + halfWidth;
        else if (xMax >= b.max.x) 
            position.x = b.max.x - halfWidth;
        // Check if the camera is outside the bounds on the y-axis
        if (yMin <= b.min.y) 
            position.y = b.min.y + halfHeight;
        else if (yMax >= b.max.y) 
            position.y = b.max.y - halfHeight;

        // Apply the clamped position
        transform.position = position;
    }
}