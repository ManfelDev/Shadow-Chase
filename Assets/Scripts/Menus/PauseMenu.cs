using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject player;
    public static bool GameIsPaused = false; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } 
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Enable player UI
        playerUI.SetActive(true);

        // Enable all scripts in player and its children components
        EnableAllScripts(player);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Disable player UI
        playerUI.SetActive(false);

        // Disable all scripts in player and its children components
        DisableAllScripts(player);
    }

    void DisableAllScripts(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();

        // Disable scripts in current GameObject
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        // Disable scripts in children components
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            DisableAllScripts(obj.transform.GetChild(i).gameObject);
        }
    }

    void EnableAllScripts(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();

        // Enable scripts in current GameObject
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }

        // Enable scripts in children components
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            EnableAllScripts(obj.transform.GetChild(i).gameObject);
        }
    }

    public void GoToSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void GoBackToPauseMenu()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    } 
}