using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int     maxHealth = 100;
    [SerializeField] private int     startingAmmo = 12;
    [SerializeField] private HealBar healBar;
    private int currentHealth;
    private int currentAmmo;
    

    // Get and set ammo
    public int Ammo { get => currentAmmo; set => currentAmmo = value; }

    // Get pistol max ammo
    public int PistolMaxAmmo { get => 12;}

    // Start is called before the first frame update
    void Start()
    {
        // Health bar setup
        currentHealth = maxHealth;
        healBar.SetMaxHeal(maxHealth);

        // Ammo setup
        currentAmmo = startingAmmo;
    }

    // Take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healBar.SetHeal(currentHealth);
    }
}
