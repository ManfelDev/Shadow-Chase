using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Get audio source
    public AudioSource AudioSource { get => audioSource; }

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
