using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private FadeManager _fadeManager;
    public static PauseMenuManager _;
    [SerializeField] private bool _debugMode;

    public enum PauseMenuButtons { Resume, MainMenu, Quit };

    [Header("Scenes")]
    [SerializeField] private string _mainMenuSceneName = "Main Menu";

    [Header("UI")]
    [SerializeField] private GameObject pauseCanvas;   // parent with Resume, Main Menu, Quit

    [Header("Audio")]
    [SerializeField] private AudioSource _clickSound;

    private bool _isPaused = false;

    private void Awake()
    {
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("There are more than 1 PauseMenuManager in the scene");
        }

        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);   // start hidden
        }

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
        }
    }

    private void DebugMessage(string message)
    {
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }

    public void PauseGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

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

    private void MainMenuClicked()
    {
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private IEnumerator FadeOutAndThenLoadSceneRealtime(string sceneName)
    {
        _fadeManager.DoFade(0, 1, 0.5f, 0);
        yield return new WaitForSecondsRealtime(0.6f);
        SceneManager.LoadScene(sceneName);
    }
}
