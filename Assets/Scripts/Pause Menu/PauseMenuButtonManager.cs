using UnityEngine;

public class PauseMenuButtonManager : MonoBehaviour
{
    [SerializeField] private PauseMenuManager.PauseMenuButtons buttonType;

    public void ButtonClicked()
    {
        if (PauseMenuManager._ != null)
        {
            PauseMenuManager._.PauseMenuButtonClicked(buttonType);
        }
        else
        {
            Debug.LogError("PauseMenuManager._ is null, is there a PauseMenuManager in this scene?");
        }
    }
}
