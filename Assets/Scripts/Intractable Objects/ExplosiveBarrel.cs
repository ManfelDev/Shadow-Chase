using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private int explosionDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Explode();
        }
    }

    private void Explode()
    {
        // Find all colliders within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // Loop through all colliders and damage any player or enemy within the explosion radius
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Get the player manager and damage them
                PlayerManager player = collider.gameObject.GetComponent<PlayerManager>();
                player.TakeDamage(explosionDamage);
            }
            else if (collider.CompareTag("Enemy"))
            {
                // Get the enemy manager and damage them
                EnemyManager enemy = collider.gameObject.GetComponent<EnemyManager>();
                enemy.TakeDamage(explosionDamage);
            }
        }

        // Destroy the barrel game object
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}