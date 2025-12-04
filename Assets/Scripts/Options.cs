using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullscreenToggle;

    [Header("Audio Sources")]
    public AudioSource musicSource; // e.g. MainMenuMusic
    public AudioSource sfxSource;   // e.g. UI Audio

    // PlayerPrefs keys
    const string MasterKey = "MasterVolume";
    const string MusicKey = "MusicVolume";
    const string SfxKey = "SfxVolume";
    const string FullKey = "Fullscreen";

    // Internal values (0–1)
    float masterVolume = 1f;
    float musicVolume = 1f;
    float sfxVolume = 1f;

    void Start()
    {
        Debug.Log("OptionsMenu Start() – loading saved settings");

        // Load saved values (default to 1)
        masterVolume = PlayerPrefs.GetFloat(MasterKey, 1f);
        musicVolume = PlayerPrefs.GetFloat(MusicKey, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SfxKey, 1f);
        bool fullscreen = PlayerPrefs.GetInt(FullKey, 1) == 1;

        // Set UI widgets if they exist
        if (masterSlider)
        {
            masterSlider.minValue = 0f;
            masterSlider.maxValue = 1f;
            masterSlider.wholeNumbers = false;
            masterSlider.value = masterVolume;
        }

        if (musicSlider)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.wholeNumbers = false;
            musicSlider.value = musicVolume;
        }

        if (sfxSlider)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.wholeNumbers = false;
            sfxSlider.value = sfxVolume;
        }

        if (fullscreenToggle)
        {
            fullscreenToggle.isOn = fullscreen;
        }

        // Apply to actual audio + screen
        ApplyVolumes();
        Screen.fullScreen = fullscreen;
    }
    private void ApplyVolumes()
    {
        ApplyMasterVolume(masterVolume);
        ApplyMusicVolume(musicVolume);
        ApplySfxVolume(sfxVolume);
    }

    // MASTER slider callback
    public void OnMasterVolumeChanged(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        Debug.Log("Master slider changed: " + masterVolume);

        ApplyVolumes();
        PlayerPrefs.SetFloat(MasterKey, masterVolume);
        PlayerPrefs.Save();
    }

    // MUSIC slider callback
    public void OnMusicVolumeChanged(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        Debug.Log("Music slider changed: " + musicVolume);

        ApplyVolumes();
        PlayerPrefs.SetFloat(MusicKey, musicVolume);
        PlayerPrefs.Save();
    }

    // SFX slider callback
    public void OnSfxVolumeChanged(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        Debug.Log("SFX slider changed: " + sfxVolume);

        ApplyVolumes();
        PlayerPrefs.SetFloat(SfxKey, sfxVolume);
        PlayerPrefs.Save();
    }

    // FULLSCREEN toggle callback
    public void OnFullscreenChanged(bool isOn)
    {
        Debug.Log("Fullscreen changed: " + isOn);
        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt(FullKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Actually push volumes to audio sources
    private void ApplyMasterVolume(float value)
    {
        // Store the value
        masterVolume = value;

        // Global volume for the whole game (all scenes)
        AudioListener.volume = value;

        // Optional: also apply to any direct references you already have
        if (musicSource) musicSource.volume = musicVolume * value;
        if (sfxSource) sfxSource.volume = sfxVolume * value;
    }

    private void ApplyMusicVolume(float value)
    {
        musicVolume = value;

        if (musicSource)
            musicSource.volume = value * masterVolume;
    }

    private void ApplySfxVolume(float value)
    {
        sfxVolume = value;

        if (sfxSource)
            sfxSource.volume = value * masterVolume;
    }
}
