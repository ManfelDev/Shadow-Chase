using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;

    private static List<Vector2> respawnedCheckpoints = new List<Vector2>();

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (respawnedCheckpoints.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(checkpointSound, 0.5f);
            gameManager.SetLastCheckPointPosition(transform.position);
            respawnedCheckpoints.Add(transform.position);
            Destroy(gameObject);
        }
    }

    // Reset respawned checkpoints
    public static void ResetRespawnedCheckpoints()
    {
        respawnedCheckpoints.Clear();
    }
}