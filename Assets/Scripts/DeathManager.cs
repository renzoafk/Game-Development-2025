using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private FadeManager _fadeManager;   // optional, for fade when reloading / main menu
    public static DeathManager _;
    [SerializeField] private bool _debugMode = false;

    [Header("Scenes")]
    [SerializeField] private string _mainMenuSceneName = "Main Menu";

    [Header("UI")]
    [SerializeField] private GameObject deathCanvas;     // whole death screen (You Died + buttons)

    [Header("Audio")]
    [SerializeField] private AudioSource _clickSound;    // optional button click SFX

    
    private bool _isShowing = false;

    public static DeathManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("More than one DeathManager found in the scene!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Hide death UI at start
        if (deathCanvas != null)
            deathCanvas.SetActive(false);
    }

    // ====== SHOW / HIDE ======

    /// <summary>
    /// Call this when the player dies.
    /// </summary>
    public void ShowDeathScreen()
    {
        if (_isShowing) return;

        _isShowing = true;
        DebugMessage("Showing death screen");

        if (deathCanvas != null)
            deathCanvas.SetActive(true);

        // freeze gameplay
        Time.timeScale = 0f;
    }

    // ====== BUTTONS ======

    // Called by "Retry" button
    public void Retry()
    {
        DebugMessage("Retry clicked");

        if (_clickSound != null)
            _clickSound.Play();

        Time.timeScale = 1f; // make sure fades & scene load work

        string sceneName = SceneManager.GetActiveScene().name;

        if (_fadeManager != null)
        {
            StartCoroutine(FadeOutAndThenLoadSceneRealtime(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    // Called by "Main Menu" button
    public void LoadMainMenu()
    {
        DebugMessage("Main Menu clicked");

        if (_clickSound != null)
            _clickSound.Play();

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

    // Called by "Quit" button
    public void QuitGame()
    {
        DebugMessage("Quit clicked");

        if (_clickSound != null)
            _clickSound.Play();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // ====== HELPERS ======

    private void DebugMessage(string msg)
    {
        if (_debugMode)
            Debug.Log("[DeathManager] " + msg);
    }

    private IEnumerator FadeOutAndThenLoadSceneRealtime(string sceneName)
    {
        // Uses your FadeManager.DoFade(startAlpha, endAlpha, duration, delayBeforeFade)
        if (_fadeManager != null)
            _fadeManager.DoFade(0f, 1f, 0.5f, 0f);

        // use realtime so it still works if timescale changes
        yield return new WaitForSecondsRealtime(0.6f);

        SceneManager.LoadScene(sceneName);
    }
}
