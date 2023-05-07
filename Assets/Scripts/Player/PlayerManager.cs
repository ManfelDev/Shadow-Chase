using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int          maxHealth = 100;
    [SerializeField] private HealthBar    healthBar;
    [SerializeField] private GameObject   player;
    [SerializeField] private AudioClip    hurtSound;
    [SerializeField] private AudioClip    deathSound;
    [SerializeField] private AudioSource  audioSource; // Use the SECOND audio source on levelManager

    private int              currentHealth;
    private WeaponsClass     currentWeapon;
    private SpriteRenderer[] spriteRenderers;

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

        // Get all sprite renderers
        spriteRenderers = player.GetComponentsInChildren<SpriteRenderer>();
    }

    // Take damage
    public void TakeDamage(int damage)
    {
        if (damage < currentHealth)
            audioSource.clip = hurtSound;
        else
            audioSource.clip = deathSound;

        audioSource.Play();
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        StartCoroutine(Blink());
    }

    // Coroutine to make player blink red for a second
    private IEnumerator Blink()
    {
        // Set all SpriteRenderer components to red
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.red;
        }

        // Wait for a second
        yield return new WaitForSeconds(0.3f);

        // Set all SpriteRenderer components back to their original color
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.white;
        }
    }

    // Change weapon
    // public void ChangeWeapon(WeaponsClass newWeapon)
    // {
    //    currentWeapon = newWeapon;
    //    Ammo = currentWeapon.MaxAmmo;
    // }
}
