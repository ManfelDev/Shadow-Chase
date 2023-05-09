using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float       destroyTime = 2f;
    [SerializeField] private float       bulletSpeed = 200f;
    [SerializeField] private Rigidbody2D rigidBody;
    
    private PlayerManager player;
    private EnemyManager  enemy;
    private GameObject    shooter;

    public GameObject Shooter 
    {
        get { return shooter; }
        set { shooter = value; }
    }

    void Start()
    {
        rigidBody.velocity = transform.right * bulletSpeed;
        player = FindObjectOfType<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the bullet hit the ground
        if (hitInfo.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        // Check if the bullet hit an enemy
        else if (hitInfo.CompareTag("Enemy") && shooter.CompareTag("Player"))
        {
            enemy = hitInfo.GetComponent<EnemyManager>();
            enemy.TakeDamage((int)player.CurrentWeapon.Damage);
            Destroy(gameObject);
        }
        // Check if the bullet hit the player
        else if (hitInfo.CompareTag("Player") && shooter.CompareTag("Enemy"))
        {
            enemy = shooter.GetComponent<EnemyManager>();
            player.TakeDamage((int)enemy.CurrentWeapon.Damage);
            Destroy(gameObject);
        }
        else if (hitInfo.CompareTag("Enemy") || hitInfo.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        // Destroy the bullet if the time is up
        else
        {
            Destroy(gameObject, destroyTime);
        }
    }
}