using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private FadeManager _fadeManager;   // optional, for fade to main menu
    public static PauseMenuManager _;
    [SerializeField] private bool _debugMode = false;

    // Added Options + OptionsBack
    public enum PauseMenuButtons
    {
        Resume,
        MainMenu,
        Quit,
        Options,
        OptionsBack
    };

    [Header("Scenes")]
    [SerializeField] private string _mainMenuSceneName = "Main Menu";

    [Header("UI")]
    [SerializeField] private GameObject pauseCanvas;        // whole pause canvas
    [SerializeField] private GameObject pauseMainPanel;     // panel with Resume / Options / Quit
    [SerializeField] private GameObject pauseOptionsPanel;  // the Options prefab instance

    [Header("Audio")]
    [SerializeField] private AudioSource _clickSound;

    private bool _isPaused = false;

    private void Awake()
    {
        // singleton
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("There are more than 1 PauseMenuManager in the scene");
        }

        // Start with pause UI hidden
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

    public void PauseMenuButtonClicked(PauseMenuButtons buttonClicked)
    {
        DebugMessage("Pause button clicked: " + buttonClicked);

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

    private void DebugMessage(string message)
    {
        if (_debugMode)
            Debug.Log(message);
    }

    // ====== PAUSE / RESUME ======

    public void PauseGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

        // show main panel, hide options when entering pause
        ShowPauseMain();

        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        Time.timeScale = 1f;
        _isPaused = false;
    }

    // ====== PANEL SWITCHING ======

    private void ShowOptions()
    {
        if (pauseMainPanel != null)
            pauseMainPanel.SetActive(false);

        if (pauseOptionsPanel != null)
            pauseOptionsPanel.SetActive(true);
    }

    private void ShowPauseMain()
    {
        if (pauseOptionsPanel != null)
            pauseOptionsPanel.SetActive(false);

        if (pauseMainPanel != null)
            pauseMainPanel.SetActive(true);
    }

    // ====== BUTTON ACTIONS ======

    private void MainMenuClicked()
    {
        DebugMessage("Returning to Main Menu");

        Time.timeScale = 1f;

        if (_fadeManager != null)
        {
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

    private IEnumerator FadeOutAndThenLoadSceneRealtime(string sceneName)
    {
        // Uses your existing FadeManager script (DoFade).
        _fadeManager.DoFade(0, 1, 0.5f, 0);
        yield return new WaitForSecondsRealtime(0.6f);
        SceneManager.LoadScene(sceneName);
    }
}
