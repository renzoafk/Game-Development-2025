using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public string gameSceneName = "SampleScene"; // put your game scene name here

    public void OnStartPressed()
    {
        if (ScreenFade.Instance != null)
        {
            // Start the fade out and load
            StartCoroutine(ScreenFade.Instance.FadeOutAndLoad(gameSceneName));
        }
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
