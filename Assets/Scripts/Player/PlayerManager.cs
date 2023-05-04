using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int     maxHealth = 100;
    [SerializeField] private HealthBar healthBar;
    private int          currentHealth;
    private WeaponsClass currentWeapon;

    // Get and set ammo
    public int Ammo { get; set; }

    // Get current weapon
    public WeaponsClass CurrentWeapon { get { return currentWeapon; } }

    // Start is called before the first frame update
    void Start()
    {
        // Health bar setup
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Weapon setup
        currentWeapon = WeaponsClass.Pistol;
        Ammo = currentWeapon.MaxAmmo;
    }

    // Take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    // Change weapon
    // public void ChangeWeapon(WeaponsClass newWeapon)
    // {
    //    currentWeapon = newWeapon;
    //    Ammo = currentWeapon.MaxAmmo;
    // }
}
