using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    // Go to How to Play
    public void GoToHowToPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    // Go to main menu
    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

}
