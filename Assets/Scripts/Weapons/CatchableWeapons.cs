using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchableWeapons : MonoBehaviour
{
    [SerializeField] private AudioClip pickUpSound;
    private PlayerManager playerManager;
    private GameObject    player;
    private GameObject    pickText;
    private bool canCollect = false;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    // Check the weapon by the tag of the weapon
    private WeaponsClass CatchableWeapon
    {
        get
        {
            switch (gameObject.tag)
            {
                case "Pistol":
                    return WeaponsClass.Pistol;
                case "AK":
                    return WeaponsClass.AK;
                default:
                    return WeaponsClass.Punch;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        pickText = player.transform.Find("PickMeText").gameObject;
    }

    private void Update()
    {
        // Check if the "E" key is pressed and the player is in proximity to the weapon
        if (canCollect && Input.GetKeyDown(KeyCode.E) && playerManager.CurrentHealth > 0)
        {
            CollectWeapon();
        }
        else if (playerManager.CurrentHealth <= 0)
        {
            canCollect = false;
            pickText.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerManager.Ammo < playerManager.CurrentWeapon.MaxAmmo ||
                playerManager.CurrentWeapon.Tag != CatchableWeapon.Tag)
            {
                canCollect = true;
                pickText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canCollect = false;
            pickText.SetActive(false);
        }
    }

    private void CollectWeapon()
    {
        // Check if player's current weapon tag is the same as the catchable weapon tag
        if (playerManager.CurrentWeapon.Tag == CatchableWeapon.Tag)
        {
            // Add ammo to the player
            playerManager.Ammo += (int)(CatchableWeapon.MaxAmmo / Random.Range(4, 5));
        }
        else
        {
            // Change the player's current weapon
            playerManager.ChangeWeapon(CatchableWeapon);
            // Set the player's ammo to the weapon's max ammo
            playerManager.Ammo = (int)(CatchableWeapon.MaxAmmo / 2);
        }

        // Play pick up sound
        audioSource.PlayOneShot(pickUpSound, 1f);

        // Destroy the weapon
        Destroy(gameObject);
    }
}