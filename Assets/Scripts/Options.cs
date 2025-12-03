using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private const string MasterKey = "Volume_Master";
    private const string MusicKey = "Volume_Music";
    private const string SfxKey = "Volume_Sfx";
    private const string FullKey = "Fullscreen";

    private void Start()
    {
        
        // PlayerPrefs.DeleteAll();
        float master = PlayerPrefs.GetFloat(MasterKey, 1f);
        float music = PlayerPrefs.GetFloat(MusicKey, 1f);
        float sfx = PlayerPrefs.GetFloat(SfxKey, 1f);
        bool fullscreen = PlayerPrefs.GetInt(FullKey, 1) == 1;

        if (masterVolumeSlider) masterVolumeSlider.value = master;
        if (musicVolumeSlider) musicVolumeSlider.value = music;
        if (sfxVolumeSlider) sfxVolumeSlider.value = sfx;
        if (fullscreenToggle) fullscreenToggle.isOn = fullscreen;

        ApplyMaster(master);
        ApplyMusic(music);
        ApplySfx(sfx);
        Screen.fullScreen = fullscreen;
    }

    // MASTER only
    public void OnMasterVolumeChanged(float value)
    {
        ApplyMaster(value);
        PlayerPrefs.SetFloat(MasterKey, value);
    }

    // MUSIC only
    public void OnMusicVolumeChanged(float value)
    {
        ApplyMusic(value);
        PlayerPrefs.SetFloat(MusicKey, value);
    }

    // SFX only
    public void OnSfxVolumeChanged(float value)
    {
        ApplySfx(value);
        PlayerPrefs.SetFloat(SfxKey, value);
    }

    public void OnFullscreenChanged(bool isOn)
    {
        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt(FullKey, isOn ? 1 : 0);
    }

    public void OnApplyPressed()
    {
        PlayerPrefs.Save();
    }

    // --- helpers ---
    private void ApplyMaster(float v)
    {
        AudioListener.volume = v;
    }

    private void ApplyMusic(float v)
    {
        if (musicSource) musicSource.volume = v;
    }

    private void ApplySfx(float v)
    {
        if (sfxSource) sfxSource.volume = v;
    }
}
