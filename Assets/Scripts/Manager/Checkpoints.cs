using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private static List<Vector2> respawnedCheckpoints = new List<Vector2>();

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
            gameManager.SetLastCheckPointPosition(transform.position);
            respawnedCheckpoints.Add(transform.position);
            Destroy(gameObject);
        }
    }
}