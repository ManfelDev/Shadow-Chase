using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int          maxHealth = 100;
    [SerializeField] private AudioClip    hurtSound;
    [SerializeField] private AudioClip    deathSound;
    [SerializeField] private GameObject   weaponToDropWhenDead;
    
    private int          currentHealth;
    private WeaponsClass currentWeapon;
    private Animator     animator;
    private bool         dead = false;

    // Get current weapon
    public WeaponsClass CurrentWeapon { get { return currentWeapon; } }

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

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
            audioSource.PlayOneShot(deathSound, 1f);
            dead = true;
            Destroy(gameObject);
            DropWeapon();
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
                audioSource.PlayOneShot(hurtSound, 1f);

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

    // Drop weapon prefab
    public void DropWeapon()
    {
        Vector3 dropPosition = transform.position;
        dropPosition.y += 7f;
        Instantiate(weaponToDropWhenDead, dropPosition, Quaternion.identity);
    }
}
