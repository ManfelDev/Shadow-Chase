using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MissionComplete : MonoBehaviour
{
    [SerializeField] private float      screenDuration = 5f;
    [SerializeField] private GameObject missionCompleteUI;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip  missionCompleteSound;

    private GameManager gameManager;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    private PauseMenu pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides with mission complete trigger
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(ShowMissionComplete());
        }
    }

    IEnumerator ShowMissionComplete()
    {
        missionCompleteUI.SetActive(true);
        Time.timeScale = 0f;

        // Disable player UI
        playerUI.SetActive(false);
        // Disable all scripts in player and its children components
        pauseMenu.DisableAllScripts(player);

        // Stop all sounds
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }

        // Play mission complete sound
        audioSource.PlayOneShot(missionCompleteSound, 1f);

        yield return new WaitForSecondsRealtime(screenDuration);

        // Reset game manager
        gameManager.ResetGameManager();
        // Reset checkpoints
        Checkpoints.ResetRespawnedCheckpoints();
        // Reset time scale
        Time.timeScale = 1f;
        // Reset the scene to the beginning
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Go to main menu
        SceneManager.LoadScene(0);
    }
}
