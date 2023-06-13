using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private PauseMenu pauseMenu;

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    // Go to How to Play
    public void GoToHowToPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    // Go to main menu from pause menu
    public void GoToMainMenuPause()
    {
        pauseMenu.Resume();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Go to main menu from how to play
    public void GoToMainMenuHowToPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
