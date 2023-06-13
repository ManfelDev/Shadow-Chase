using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{   
    [SerializeField] private Transform  puchPoint;
    [SerializeField] private float      punchRange = 3f;
    [SerializeField] private AudioClip  punchSound;
    [SerializeField] private AudioClip  punchHitSound;
    private EnemyManager enemy;
    private WeaponsClass punch;
    private SoundManager soundManager;

    void Awake()
    {
        punch = WeaponsClass.Punch;
        soundManager = FindObjectOfType<SoundManager>();
    }

    // If collide with an enemy, deal damage
    public void Punch()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(puchPoint.position, punchRange);
        soundManager.PlaySound(punchSound, 1f);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyManager>().TakeDamage((int)punch.Damage);
                soundManager.PlaySound(punchHitSound, 1f);
            }
        }
    }   

    // Draw the punch range
    void OnDrawGizmosSelected()
    {
        if (puchPoint == null)
            return;

        Gizmos.DrawWireSphere(puchPoint.position, punchRange);
    }
}