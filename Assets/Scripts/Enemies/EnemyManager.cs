using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int          maxHealth = 100;
    [SerializeField] private AudioClip    hurtSound;
    [SerializeField] private AudioClip    deathSound;
    [SerializeField] private AudioSource  audioSource; // Use the SECOND audio source in the enemy prefab

    private int          currentHealth;
    private WeaponsClass currentWeapon;
    private Animator     animator;
    private bool         dead = false;

    // Get current weapon
    public WeaponsClass CurrentWeapon { get { return currentWeapon; } }

    // Start is called before the first frame update
    void Start()
    {
        // Health bar setup
        currentHealth = maxHealth;

        // Weapon setup
        currentWeapon = WeaponsClass.AK;

        // Animations setup
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            dead = true;
            currentHealth = 0;
        }

        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Take damage
    public void TakeDamage(int damage)
    {
        if (!dead)
        {
            if (damage < currentHealth)
                audioSource.clip = hurtSound;
            else
                audioSource.clip = deathSound;

            audioSource.Play();
            currentHealth -= damage;
        }
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool GetDead()
    {
        return dead;
    }
}
