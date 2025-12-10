using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private FadeManager _fadeManager;   // optional fade
    [SerializeField] private bool _debugMode = false;

    public static DeathManager Instance { get; private set; }

    [Header("Scenes")]
    [SerializeField] private string _mainMenuSceneName = "Main Menu";

    [Header("UI")]
    [SerializeField] private GameObject _deathCanvas;    // YOU DIED screen root
    [SerializeField] private GameObject _playerHealthUI; // <-- NEW: health bar / HUD root

    [Header("Audio")]
    [SerializeField] private AudioSource _clickSound;    // optional button click SFX

    private bool _isShowing = false;

    private void Awake()
    {
        // simple singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_deathCanvas != null)
            _deathCanvas.SetActive(false);
    }

    // --------------------------------------------------
    // Called by PlayerHealth when HP hits 0
    // --------------------------------------------------
    public void ShowDeathScreen()
    {
        if (_isShowing) return;
        _isShowing = true;

        DebugMessage("ShowDeathScreen");

        // hide gameplay UI (health bar etc.)
        if (_playerHealthUI != null)
            _playerHealthUI.SetActive(false);

        if (_deathCanvas != null)
            _deathCanvas.SetActive(true);

        Time.timeScale = 0f;
    }

    // --------------------------------------------------
    // Buttons
    // --------------------------------------------------
    public void Retry()
    {
        DebugMessage("Retry clicked");
        PlayClick();

        Time.timeScale = 1f;

        string sceneName = SceneManager.GetActiveScene().name;

        if (_fadeManager != null)
            StartCoroutine(FadeOutAndThenLoadSceneRealtime(sceneName));
        else
            SceneManager.LoadScene(sceneName);
    }

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

    // --------------------------------------------------
    // Helpers
    // --------------------------------------------------
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
        if (_fadeManager != null)
            _fadeManager.DoFade(0f, 1f, 0.5f, 0f);

        yield return new WaitForSecondsRealtime(0.6f);
        SceneManager.LoadScene(sceneName);
    }
}
