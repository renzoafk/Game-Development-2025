using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private FadeManager _fadeManager;
    public static MainMenuManager _;

    [SerializeField] private bool _debugMode;

    public enum MainMenuButtons { play, options, credits, quit };
    [SerializeField] private string _sceneToLoadAfterClickingPlay;

    [SerializeField] private GameObject mainMenuPanel;    // NEW
    [SerializeField] private GameObject optionsPanel;      // NEW
    [SerializeField] private GameObject creditsPanel;      // NEW

    [SerializeField] AudioSource PlaySound;

    public void Awake()
    {
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("There are more that 1 MainMenuManager's in the scene");
        }
    }
    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage("Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case MainMenuButtons.play:
                PlayClicked();
                break;
            case MainMenuButtons.options:
                OptionsClicked();
                break;
            case MainMenuButtons.credits:
                CreditsClicked();
                break;
            case MainMenuButtons.quit:
                QuitGame();
                break;
        }
    }
    public void DebugMessage(string message)
    {
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }
    public void OptionsClicked()
    {
        DebugMessage("Opening OPTIONS menu...");
        StartCoroutine(FadeSwapPanels(mainMenuPanel, optionsPanel));
    }
    public void BackToMainMenu()
    {
        DebugMessage("Going back to MAIN MENU...");
        if (optionsPanel.activeSelf)
            StartCoroutine(FadeSwapPanels(optionsPanel, mainMenuPanel));

        else if (creditsPanel.activeSelf)
            StartCoroutine(FadeSwapPanels(creditsPanel, mainMenuPanel));
    }
    public void CreditsClicked()
    {
        DebugMessage("Opening CREDITS menu...");
        StartCoroutine(FadeSwapPanels(mainMenuPanel, creditsPanel));
    }
    public void PlayClicked()
    {
        PlaySound.Play();
        StartCoroutine(FadeOutAndThenLoadScene("SampleScene"));
        //SceneManager.LoadScene(_sceneToLoadAfterClickingPlay);
    }

    IEnumerator FadeOutAndThenLoadScene(string sceneName)
    {
        _fadeManager.DoFade(0, 2, 2, 0);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    private IEnumerator FadeSwapPanels(GameObject from, GameObject to)
    {
        // Fade to black
        _fadeManager.DoFade(0, 1, 0.5f, 0);
        yield return new WaitForSeconds(0.6f);

        // Switch panels while black
        from.SetActive(false);
        to.SetActive(true);

        // Fade back in
        _fadeManager.DoFade(1, 0, 0.5f, 0);
        yield return null;
    }


}
