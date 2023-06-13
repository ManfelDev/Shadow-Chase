using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceSound;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip menuMusic;

    private float lastMusic = -120f;

    // Get audio source
    public AudioSource AudioSourceSound { get => audioSourceSound; }

    void Update ()
    {
        if (lastMusic + 115f < Time.time)
        {
            audioSourceMusic.PlayOneShot(menuMusic, 0.4f);
            lastMusic = Time.time;
        }
    }

    // Play a click sound
    public void PlayClickSound(float volume = 1f)
    {
        audioSourceSound.PlayOneShot(clickSound, volume);
    }
}
