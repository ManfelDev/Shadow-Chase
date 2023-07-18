using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int          maxHealth = 100;
    [SerializeField] private HealthBar    healthBar;
    [SerializeField] private GameObject   player;
    [SerializeField] private AudioClip    hurtSound;
    [SerializeField] private AudioClip    deathSound;
    [SerializeField] private bool         cheats = false;
    [SerializeField] private GameObject   debugEnemy;

    private int              currentHealth;
    private WeaponsClass     currentWeapon;
    private PlayerMovement   playerMovement;
    private PlayerShooting   playerShooting;
    private SpriteRenderer[] spriteRenderers;
    private GameManager      gameManager;
    private GameObject       pickMeText;
    private EnemyAlarm       alarm;

    // Get and set ammo
    public int          Ammo { get; set; }
    // Get player's current health
    public int          CurrentHealth { get => currentHealth; private set => currentHealth = value; }
    // Get player's max health
    public int          MaxHealth { get => maxHealth; private set => maxHealth = value; }
    // Get current weapon
    public WeaponsClass CurrentWeapon 
    { 
        get => currentWeapon;
        set => currentWeapon = value;
    }
    // Get sound manager's audio source
    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

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

        // Get game manager
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // Get pick me text
        pickMeText = player.transform.Find("PickMeText").gameObject;

        // Get the enemy alarm state (for debugging)
        alarm = FindObjectOfType<EnemyAlarm>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            // Get player movement script
            playerMovement = player.GetComponent<PlayerMovement>();
            // Get player shooting script
            playerShooting = player.GetComponent<PlayerShooting>();

            // Change animation to death
            playerMovement.ChangeAnimationState("Player_Death");
            // Turn of player movement
            playerMovement.enabled = false;
            // Turn off player shooting
            playerShooting.enabled = false;
            // Take of rigid body material
            player.GetComponent<Rigidbody2D>().sharedMaterial = null;
            TurnOffPlayerSprites();

            // Restart level after 2 seconds
            StartCoroutine(RestartLevelAfterDelay(2f));
        }

        // Can't have more ammo than the weapon's max ammo
        if (Ammo > currentWeapon.MaxAmmo)
        {
            Ammo = currentWeapon.MaxAmmo;
        }

        // Prevent pick me text from rotating
        if (player.transform.rotation.y == -180)
        {
            pickMeText.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            pickMeText.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetButtonDown("Testing Key 1") && cheats)
        {
            Debug.Log("Teleported to Spawn");
            player.transform.position = new Vector3(-110, -30, 0);
        }

        if (Input.GetButtonDown("Testing Key 2") && cheats)
        {
            Debug.Log("Teleported to Construction Checkpoint");
            player.transform.position = new Vector3(9324, 45, 0);
        }

        if (Input.GetButtonDown("Testing Key 3") && cheats)
        {
            Debug.Log("Teleported to End");
            player.transform.position = new Vector3(13743, 50, 0);
        }

        if (Input.GetButtonDown("Testing Key 4") && cheats)
        {
            Debug.Log("Alarm Triggered");
            alarm.Trigger();
        }

        if (Input.GetButtonDown("Testing Key 5") && cheats)
        {
            Debug.Log("Spawning Debug Enemy");
            Vector3 spawnPosition = new Vector3(player.transform.position.x, player.transform.position.y + 40f, 0);
            Instantiate(debugEnemy, spawnPosition, player.transform.rotation);
        }

        if (Input.GetButtonDown("Testing Key 6"))
        {
            cheats = true;
            if (MaxHealth < 10000000)
            {
                Debug.Log("GodMode activated");

                MaxHealth += 10000000;
                CurrentHealth += 10000000;
                ChangeWeapon(new WeaponsClass(0.05f, 35, 10000000, "AK", true,
                                    Resources.Load<Sprite>("Weapons/ak"),
                                    Resources.Load<Sprite>("Arms/AK arms/right_arm_ak"),
                                    Resources.Load<Sprite>("Arms/AK arms/left_arm_ak"),
                                    Resources.Load<AudioClip>("Audio/akshot"),
                                    Resources.Load<AudioClip>("Audio/AK empty")));
                Ammo += 10000000;
            }
        }
    }

    public void TurnOffPlayerSprites()
    {
        // Turn off player's sprite renderer
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // All sprite renderers except the player's body
            if (spriteRenderer.gameObject.name != "Player")
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    public void TurnOnPlayerSprites()
    {
        // Turn on player's sprite renderer
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // All sprite renderers except the player's body
            if (spriteRenderer.gameObject.name != "Player")
            {
                spriteRenderer.enabled = true;
            }
        }
    }

    // Take damage
    public void TakeDamage(int damage)
    {
        if (damage < currentHealth)
            audioSource.PlayOneShot(hurtSound, 1f);
        else
            audioSource.PlayOneShot(deathSound, 1f);
            
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        StartCoroutine(Blink());
    }

    // Give health
    public void GiveHealth(int health)
    {
        currentHealth += health;

        healthBar.SetHealth(currentHealth);
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
    public void ChangeWeapon(WeaponsClass newWeapon)
    {
        currentWeapon = newWeapon;
        Ammo = currentWeapon.MaxAmmo;
        
        // Change the right arm sprite
        SpriteRenderer rightArmRenderer = player.transform.Find("Arms/right_arm").GetComponent<SpriteRenderer>();
        rightArmRenderer.sprite = currentWeapon.RightArmSprite;
        // Change the left arm sprite
        SpriteRenderer leftArmRenderer = player.transform.Find("Arms/left_arm").GetComponent<SpriteRenderer>();
        leftArmRenderer.sprite = currentWeapon.LeftArmSprite;
    }

    // Restart the level after a delay
    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restart the level from the last checkpoint
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}