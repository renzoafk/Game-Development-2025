using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "SampleScene"; // Game scene to load

    [Header("Menu Panels")]
    public GameObject mainMenuPanel;    // The main menu (Start/Quit/Options/Credits buttons)
    public GameObject optionsPanel;     // Options UI
    public GameObject creditsPanel;     // Credits UI

    // START GAME
    public void OnStartPressed()
    {
        if (ScreenFade.Instance != null)
        {
            StartCoroutine(ScreenFade.Instance.FadeOutAndLoad(gameSceneName));
        }
        else
        {
            // Fallback if you have no fade system
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
        }
    }

    // QUIT GAME
    public void OnQuitPressed()
    {
        Application.Quit();
    }

    // OPEN OPTIONS
    public void OnOptionsPressed()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    // BACK FROM OPTIONS
    public void OnOptionsBackPressed()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // OPEN CREDITS
    public void OnCreditsPressed()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    // BACK FROM CREDITS
    public void OnCreditsBackPressed()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }
}

