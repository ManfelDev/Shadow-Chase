using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int          currentHealth;
    private WeaponsClass currentWeapon;

    // Get current weapon
    public WeaponsClass CurrentWeapon { get { return currentWeapon; } }

    // Start is called before the first frame update
    void Start()
    {
        // Health bar setup
        currentHealth = maxHealth;

        // Weapon setup
        currentWeapon = WeaponsClass.AK;
    }

    void Update()
    {
        if (currentHealth < 0)
        {
            Destroy(gameObject);
        }

        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
