using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject HowToPlayScreen;

    public void LoadLevel(int levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    public void GoFromMainMenuToSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void GoFromSettingsToMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GoFromMainMenuToHowToPlay()
    {
        mainMenu.SetActive(false);
        HowToPlayScreen.SetActive(true);
    }

    public void GoFromHowToPlayToMainMenu()
    {
        HowToPlayScreen.SetActive(false);
        mainMenu.SetActive(true);
    }

    IEnumerator LoadLevelASync (int levelToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);

        yield return null;
    }
}
