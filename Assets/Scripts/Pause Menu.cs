using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseCanvas;

    private bool isPaused = false;

    void Awake()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);   // start hidden
        }
    }

    void Update()
    {
        // ESC toggles pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

        Time.timeScale = 0f;   // freeze gameplay
        isPaused = true;

        // Optional: lock the cursor, etc.
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        Time.timeScale = 1f;   // resume gameplay
        isPaused = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;   // make sure timeScale is normal again
        SceneManager.LoadScene("Main Menu");  // change to your main-menu scene name
    }

    // Optional – full quit (for builds)
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}




