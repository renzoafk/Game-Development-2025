using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance { get; private set; }

    [Header("General")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private FadeManager fadeManager;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "Main Menu";
    [SerializeField] private string nextLevelSceneName = "";  // optional, for victory -> next level

    [Header("UI - Screens")]
    [SerializeField] private GameObject deathCanvas;    // death screen UI
    [SerializeField] private GameObject victoryCanvas;  // victory screen UI

    [Header("UI - Things to hide on end screen")]
    [SerializeField] private GameObject[] hideOnEndScreens;  // e.g. Play health bar, HUD bits

    [Header("UI - Other")]
    [SerializeField] private GameObject pauseCanvas;    // your pause / gameplay HUD canvas

    [Header("Audio")]
    [SerializeField] private AudioSource clickSound;

    private bool endScreenShowing = false;

    // -------------------------------  Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (deathCanvas != null)   deathCanvas.SetActive(false);
        if (victoryCanvas != null) victoryCanvas.SetActive(false);
    }

    // -------------------------------  Common helper for both death & victory
    private void ShowEndScreen(GameObject screenToShow)
    {
        if (endScreenShowing) return;
        endScreenShowing = true;

        // Hide pause / HUD canvas if you have one
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        // Hide any extra UI bits like the health bar
        if (hideOnEndScreens != null)
        {
            foreach (var go in hideOnEndScreens)
            {
                if (go != null) go.SetActive(false);
            }
        }

        // Freeze gameplay, but UI still works
        Time.timeScale = 0f;

        if (screenToShow != null)
            screenToShow.SetActive(true);

        DebugMsg("Showing end screen: " + (screenToShow != null ? screenToShow.name : "none"));
    }

    // -------------------------------  Called by PlayerHealth
    public void ShowDeathScreen()
    {
        ShowEndScreen(deathCanvas);
    }

    // -------------------------------  Called by your Victory_trigger
    public void ShowVictoryScreen()
    {
        ShowEndScreen(victoryCanvas);
    }

    // -------------------------------  Buttons

    public void Retry()
    {
        DebugMsg("Retry clicked");
        PlayClick();

        Time.timeScale = 1f; // unfreeze

        string currentScene = SceneManager.GetActiveScene().name;
        StartCoroutine(FadeAndLoadScene(currentScene));
    }

    public void LoadMainMenu()
    {
        DebugMsg("Main Menu clicked");
        PlayClick();

        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogError("[DeathManager] Main menu scene name not set!");
            return;
        }

        Time.timeScale = 1f;
        StartCoroutine(FadeAndLoadScene(mainMenuSceneName));
    }

    // Optional “Next Level” button on victory screen
    public void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextLevelSceneName))
        {
            Debug.LogError("[DeathManager] Next level scene name not set!");
            return;
        }

        DebugMsg("Next level clicked");
        PlayClick();

        Time.timeScale = 1f;
        StartCoroutine(FadeAndLoadScene(nextLevelSceneName));
    }

    public void QuitGame()
    {
        DebugMsg("Quit clicked");
        PlayClick();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // -------------------------------  Helpers

    private void PlayClick()
    {
        if (clickSound != null)
            clickSound.Play();
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadeManager != null)
            fadeManager.DoFade(0f, 1f, 0.5f, 0f);

        // realtime so it still waits even if Time.timeScale == 0
        yield return new WaitForSecondsRealtime(0.6f);

        SceneManager.LoadScene(sceneName);
    }

    private void DebugMsg(string msg)
    {
        if (debugMode)
            Debug.Log("[DeathManager] " + msg);
    }
}
