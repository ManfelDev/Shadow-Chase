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
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    // Go to main menu
    public void GoToMainMenuPause()
    {
        pauseMenu.Resume();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
