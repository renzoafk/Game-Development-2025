using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    public void PlayHover()
    {
        if (hoverClip != null)
        {
            audioSource.PlayOneShot(hoverClip);
        }
    }

    public void PlayClick()
    {
        if (clickClip != null)
        {
            audioSource.PlayOneShot(clickClip);
        }
    }
}