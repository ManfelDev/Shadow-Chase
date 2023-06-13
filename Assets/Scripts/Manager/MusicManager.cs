using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceAlarm;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioClip alarmSound;
    [SerializeField] private AudioClip stealthMusic;
    [SerializeField] private AudioClip actionMusic;


    // Get audio source
    public AudioSource AudioSourceAlarm { get => audioSourceAlarm; }
    public AudioSource AudioSourceMusic { get => audioSourceMusic; }

    private EnemyAlarm alarm;

    private float lastMusic = -200;
    private float lastAlarm = -50;
    private bool alarmed = false;

    // Start is called before the first frame update
    void Awake()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSourceAlarm = audioSources[0];
        audioSourceMusic = audioSources[1];

        audioSourceAlarm.playOnAwake = false; // Disables auto-play for the alarm

        alarm = GameObject.FindObjectOfType<EnemyAlarm>();
    }

    void Update()
    {
        if (alarmed && lastAlarm + 30f < Time.time)
        {
            PlayAlarm();
            lastAlarm = Time.time;
        }

        if (alarmed && lastMusic + 145f < Time.time)
        {
            PlayMusic(actionMusic);
            lastMusic = Time.time;
        }
        else if (!alarmed && lastMusic + 110f < Time.time)
        {
            PlayMusic(stealthMusic, 0.6f);
            lastMusic = Time.time;
        }

        if (alarm.IsON && !alarmed)
        {
            alarmed = true;
            audioSourceMusic.Stop();
            lastMusic = -150;
            lastAlarm = -40;
        }
    }

    // Play a given music
    public void PlayMusic(AudioClip clip, float volume = 0.5f)
    {
        audioSourceMusic.PlayOneShot(clip, volume);
    }

    // Play the alarm
    public void PlayAlarm(float volume = 0.5f)
    {
        audioSourceAlarm.PlayOneShot(alarmSound, 0.5f);
    }
}
