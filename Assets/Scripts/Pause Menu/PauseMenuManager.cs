using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private FadeManager _fadeManager;
    [SerializeField] private bool _debugMode = false;
    public static PauseMenuManager _;

    // IMPORTANT: now includes Options + OptionsBack
    public enum PauseMenuButtons
    {
        Resume,
        MainMenu,
        Quit,
        Options,
        OptionsBack
    }

    [Header("Scenes")]
    [SerializeField] private string _mainMenuSceneName = "Main Menu";

    [Header("UI")]
    [SerializeField] private GameObject pauseCanvas;        // whole pause canvas
    [SerializeField] private GameObject pauseMainPanel;     // panel with Resume / Options / Main / Quit
    [SerializeField] private GameObject pauseOptionsPanel;  // the Options prefab instance

    [Header("Audio")]
    [SerializeField] private AudioSource _clickSound;

    private bool _isPaused = false;

    private void Awake()
    {
        if (_ != null && _ != this)
        {
            Debug.LogError("There is more than one PauseMenuManager in this scene.");
        }
        _ = this;

        // Make sure pause UI starts hidden
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        if (pauseOptionsPanel != null)
            pauseOptionsPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

        // When we pause, show main panel and hide options
        if (pauseMainPanel != null)
            pauseMainPanel.SetActive(true);
        if (pauseOptionsPanel != null)
            pauseOptionsPanel.SetActive(false);

        Time.timeScale = 0f;
        _isPaused = true;
        DebugMessage("Game paused");
    }

    public void ResumeGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        Time.timeScale = 1f;
        _isPaused = false;
        DebugMessage("Game resumed");
    }

    public void PauseMenuButtonClicked(PauseMenuButtons buttonClicked)
    {
        DebugMessage("Pause menu button clicked: " + buttonClicked);

        if (_clickSound != null)
            _clickSound.Play();

        switch (buttonClicked)
        {
            case PauseMenuButtons.Resume:
                ResumeGame();
                break;

            case PauseMenuButtons.MainMenu:
                MainMenuClicked();
                break;

            case PauseMenuButtons.Quit:
                QuitGame();
                break;

            case PauseMenuButtons.Options:
                ShowOptions();
                break;

            case PauseMenuButtons.OptionsBack:
                ShowPauseMain();
                break;
        }
    }

    private void ShowOptions()
    {
        DebugMessage("Show Options panel");
        if (pauseMainPanel != null)
            pauseMainPanel.SetActive(false);
        if (pauseOptionsPanel != null)
            pauseOptionsPanel.SetActive(true);
    }

    private void ShowPauseMain()
    {
        DebugMessage("Back from Options to main pause panel");
        if (pauseOptionsPanel != null)
            pauseOptionsPanel.SetActive(false);
        if (pauseMainPanel != null)
            pauseMainPanel.SetActive(true);
    }

    // =========================
    // BUTTON ACTIONS
    // =========================

    private void MainMenuClicked()
    {
        DebugMessage("Returning to Main Menu");

        // Un-pause before scene change
        Time.timeScale = 1f;

        if (_fadeManager != null)
        {
            // CORRECT: use the coroutine you already have
            StartCoroutine(FadeOutAndThenLoadSceneRealtime(_mainMenuSceneName));
        }
        else
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }
    }

    private void QuitGame()
    {
        DebugMessage("Quitting Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // =========================
    // FADE + LOAD (REALTIME)
    // =========================

    private IEnumerator FadeOutAndThenLoadSceneRealtime(string sceneName)
    {
        if (_fadeManager != null)
            _fadeManager.DoFade(0, 1, 0.5f, 0);

        // Wait using realtime so pause Time.timeScale = 0 doesn't matter
        yield return new WaitForSecondsRealtime(0.6f);

        SceneManager.LoadScene(sceneName);
    }

    // =========================
    // DEBUG HELPER
    // =========================

    private void DebugMessage(string msg)
    {
        if (_debugMode)
            Debug.Log("[PauseMenuManager] " + msg);
    }
}
