using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private FadeManager _fadeManager;   // same FadeManager you use for death
    [SerializeField] private bool _debugMode = false;

    public static VictoryManager Instance { get; private set; }

    [Header("Scenes")]
    [SerializeField] private string _mainMenuSceneName = "Main Menu";

    [Header("UI")]
    [SerializeField] private GameObject _victoryCanvas;
    [SerializeField] private GameObject _playerHealthUI;

    private bool _hasWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (_victoryCanvas != null)
            _victoryCanvas.SetActive(false);
    }

    public void ShowVictoryScreen()
    {
        if (_hasWon) return;
        _hasWon = true;

        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        // hide player HUD
        if (_playerHealthUI != null)
            _playerHealthUI.SetActive(false);

        // fade, same as DeathManager
        if (_fadeManager != null)
            _fadeManager.DoFade(0f, 1f, 0.5f, 0f);

        yield return new WaitForSecondsRealtime(0.6f);

        Time.timeScale = 0f;              // freeze game
        _victoryCanvas.SetActive(true);   // show win screen

        Log("Victory screen shown.");
    }

    // ---------- Button hooks ----------

    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeOutAndThenLoadSceneRealtime(SceneManager.GetActiveScene().name));
    }

    public void OnClickMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeOutAndThenLoadSceneRealtime(_mainMenuSceneName));
    }

    public void OnClickQuit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    // ---------- Helpers ----------

    private void Log(string msg)
    {
        if (_debugMode)
            Debug.Log("[VictoryManager] " + msg);
    }

    private IEnumerator FadeOutAndThenLoadSceneRealtime(string sceneName)
    {
        if (_fadeManager != null)
            _fadeManager.DoFade(0f, 1f, 0.5f, 0f);

        yield return new WaitForSecondsRealtime(0.6f);
        SceneManager.LoadScene(sceneName);
    }
}
