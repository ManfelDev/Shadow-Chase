using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{   
    [SerializeField] private Transform  puchPoint;
    [SerializeField] private float      punchRange = 3f;
    private EnemyManager enemy;
    private WeaponsClass punch;

    void Awake()
    {
        punch = WeaponsClass.Punch;
    }

    // If collide with an enemy, deal damage
    public void Punch()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(puchPoint.position, punchRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyManager>().TakeDamage((int)punch.Damage);
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