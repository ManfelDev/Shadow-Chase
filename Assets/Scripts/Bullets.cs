using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private float bulletSpeed = 200f;

    void Start()
    {
        rigidBody.velocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // TODO: Add enemy collision
        if (hitInfo.name != "Player" && hitInfo.name != "Bounds")
        {
            Destroy(gameObject);
        }
        else
            Destroy(gameObject, destroyTime);
    }
}
