using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    // Create a variable to hold the move speed
    [SerializeField] private float moveSpeed = 100.0f;

    // Create a variable to hold the rigidbody component
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Put the current velocity into a variable
        Vector2 currentVelocity = rb.velocity;
        // Set the x velocity to the horizontal input times the move speed
        currentVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;
        // Set the rigidbody's velocity to the current velocity
        rb.velocity = currentVelocity;
    }
}
