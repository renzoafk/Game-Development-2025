using UnityEngine;

public class SceneMusicVolumeSync : MonoBehaviour
{
    public AudioSource musicSource;

    const string MasterKey = "MasterVolume";
    const string MusicKey = "MusicVolume";

    void Start()
    {
        // Load saved values (same keys as OptionsMenu)
        float master = PlayerPrefs.GetFloat(MasterKey, 1f);
        float music = PlayerPrefs.GetFloat(MusicKey, 1f);

        // Apply global master (affects all scenes)
        AudioListener.volume = master;

        // Apply music volume for THIS scene's music source
        if (musicSource != null)
        {
            musicSource.volume = master * music;
        }
    }
}
