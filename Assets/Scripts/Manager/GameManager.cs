using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector2 lastCheckPointPosition;
    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    // Set the last checkpoint position
    public void SetLastCheckPointPosition(Vector2 position)
    {
        lastCheckPointPosition = position;
    }

    // Get the last checkpoint position 
    public Vector2 LastCheckPointPosition
    {
        get
        {
            return lastCheckPointPosition;
        }
    }

    // Reset game manager
    public void ResetGameManager()
    {
        Destroy(gameObject);
    }
}
