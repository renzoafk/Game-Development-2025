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
                break;
            case MainMenuButtons.credits:
                break;
            case MainMenuButtons.quit:
                QuitGame();
                break;
            default:
                Debug.Log("Button clicked that wasn't implemented in MainMenuManager Method");
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

}
