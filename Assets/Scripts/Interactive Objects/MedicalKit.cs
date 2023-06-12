using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalKit : MonoBehaviour
{
    [SerializeField] private int       healthPoints = 50;
    [SerializeField] private AudioClip healSound;
    private PlayerManager playerManager;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // If player collide with the medical kit, add health to the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerManager.CurrentHealth < playerManager.MaxHealth)
            {
                playerManager.GiveHealth(healthPoints);
                if (playerManager.CurrentHealth > playerManager.MaxHealth)
                {
                    playerManager.GiveHealth(playerManager.MaxHealth - playerManager.CurrentHealth);
                }

                audioSource.PlayOneShot(healSound, 1f);

                Destroy(gameObject);
            }
        }
    }
}
