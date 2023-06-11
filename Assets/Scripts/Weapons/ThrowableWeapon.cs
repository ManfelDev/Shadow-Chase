using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
    [SerializeField] private float       destroyTime = 2f;
    [SerializeField] private float       throwSpeed = 200f;
    [SerializeField] private float       rotationSpeed = 5f;
    [SerializeField] private Rigidbody2D rigidBody;
    private EnemyManager   enemy;
    public SpriteRenderer  spriteRenderer;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = transform.right * throwSpeed;
    }

    // Rotate the throwable weapon until it is destroyed
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed);
    }

    // If coliision is detected
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the bullet hit the ground
        if (hitInfo.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        // Check if the bullet hit an enemy
        else if (hitInfo.CompareTag("Enemy"))
        {
            enemy = hitInfo.GetComponent<EnemyManager>();
            enemy.TakeDamage(50);
            Destroy(gameObject);
        }
        // Destroy the bullet if the time is up
        else
        {
            Destroy(gameObject, destroyTime);
        }
    }

    // Change the sprite to the throwable weapon sprite
    public void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
