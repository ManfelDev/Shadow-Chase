using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private GameObject  explosionPrefab;
    [SerializeField] private float       explosionRadius;
    [SerializeField] private int         explosionDamage;
    [SerializeField] private AudioClip   explosionSound;

    private PlayerManager player;
    private EnemyManager  enemy;
    private Box           box;
    private EnemyAlarm    alarm;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    private void Explosion()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Damage player
                player = FindObjectOfType<PlayerManager>();
                player.TakeDamage(explosionDamage);
            }
            else if (collider.CompareTag("Enemy"))
            {
                // Damage enemy
                enemy = collider.GetComponent<EnemyManager>();
                enemy.TakeDamage(explosionDamage);
            }
            else if (collider.CompareTag("Box"))
            {
                // Damage box
                box = collider.GetComponent<Box>();
                box.HitPoints = 0;
            }
        }
    }

    private void ExplosionAnimation()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            alarm = FindObjectOfType<EnemyAlarm>();
            audioSource.PlayOneShot(explosionSound, 1f);

            Destroy(collision.gameObject);
            Explosion();
            ExplosionAnimation();
            Destroy(gameObject);
            alarm.SlowTriggerAll();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}