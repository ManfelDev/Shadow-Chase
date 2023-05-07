using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private BoxCollider2D cameraBounds;
    [SerializeField] private Transform     target;
    [SerializeField] private Vector3       offset;
    [SerializeField] private float         smoothSpeed;

    private Vector3        velocity = Vector3.zero;
    private FollowMouse    followMouse;

    private new Camera     camera;

    void Awake()
    {
        camera = GetComponent<Camera>();
        followMouse = FindObjectOfType<FollowMouse>();
    }

    void FixedUpdate()
    {
        // Return if the target is null
        if (target == null) 
            return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Set the offset.x value if the player is moving right
        if (followMouse.PointingRight())
        {
            offset.x = Mathf.Abs(offset.x);
        }
        // Negate the offset.x value if the player is moving left
        else if (!followMouse.PointingRight())
        {
            offset.x = -Mathf.Abs(offset.x);
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