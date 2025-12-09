using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [Header("General")]
    public static DeathManager Instance { get; private set; }  // Singleton
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private FadeManager _fadeManager;         // optional

    [Header("Scenes")]
    [SerializeField] private string _mainMenuSceneName = "Main Menu";

    [Header("UI")]
    [SerializeField] private GameObject _deathCanvas;          // death screen (You Died + buttons)

    [Header("Audio")]
    [SerializeField] private AudioSource _clickSound;          // optional button click SFX

    private bool _isShowing = false;

    private void Awake()
    {
        // basic singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // make sure death screen starts hidden
        if (_deathCanvas != null)
            _deathCanvas.SetActive(false);
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

        if (_deathCanvas != null)
            _deathCanvas.SetActive(true);

        // freeze gameplay
        Time.timeScale = 0f;
    }

    // ====== BUTTONS ======

    // Retry button
    public void Retry()
    {
        DebugMessage("Retry clicked");
        PlayClick();

        Time.timeScale = 1f; // unfreeze so fade & load work

        string sceneName = SceneManager.GetActiveScene().name;

        if (_fadeManager != null)
            StartCoroutine(FadeOutAndThenLoadSceneRealtime(sceneName));
        else
            SceneManager.LoadScene(sceneName);
    }

    // Main Menu button
    public void LoadMainMenu()
    {
        DebugMessage("Main Menu clicked");
        PlayClick();

        Time.timeScale = 1f;

        if (string.IsNullOrEmpty(_mainMenuSceneName))
        {
            Debug.LogError("[DeathManager] Main menu scene name not set!");
            return;
        }

        if (_fadeManager != null)
            StartCoroutine(FadeOutAndThenLoadSceneRealtime(_mainMenuSceneName));
        else
            SceneManager.LoadScene(_mainMenuSceneName);
    }

    // Quit button
    public void QuitGame()
    {
        DebugMessage("Quit clicked");
        PlayClick();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ====== HELPERS ======

    private void PlayClick()
    {
        if (_clickSound != null)
            _clickSound.Play();
    }

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

        // realtime wait so it still works if timescale changes
        yield return new WaitForSecondsRealtime(0.6f);

        SceneManager.LoadScene(sceneName);
    }
}
