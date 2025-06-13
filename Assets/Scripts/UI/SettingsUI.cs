using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider mouseSensitivitySlider;

    private bool _initialized = false;

    private void Start()
    {
        LoadSettings();

        // Event binding
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);

        _initialized = true;
    }

    private void OnMasterVolumeChanged(float value)
    {
        if (!_initialized) return;

        AudioManager.Instance.SetMasterVolume(value);
        //PlayerPrefs.SetFloat("MasterVolume", value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        if (!_initialized) return;

        AudioManager.Instance.SetMusicVolume(value);
        //PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (!_initialized) return;

        AudioManager.Instance.SetSFXVolume(value);
        //PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void OnMouseSensitivityChanged(float value)
    {
        if (!_initialized) return;

        GameManager.Instance.MouseSensitivity = value;
        //PlayerPrefs.SetFloat("MouseSensitivity", value);
    }

    private void LoadSettings()
    {
        float master = 0.5f;
        float music = 0.5f;
        float sfx = 0.5f;
        float sens = 1.0f; 
        
        masterVolumeSlider.value = master;
        musicVolumeSlider.value = music;
        sfxVolumeSlider.value = sfx;
        mouseSensitivitySlider.value = sens;
        
        AudioManager.Instance.SetMasterVolume(master);
        AudioManager.Instance.SetMusicVolume(music);
        AudioManager.Instance.SetSFXVolume(sfx);
        GameManager.Instance.MouseSensitivity = sens;
    }

    /*
    private void LoadSettings()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float sens = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        masterVolumeSlider.value = master;
        musicVolumeSlider.value = music;
        sfxVolumeSlider.value = sfx;
        mouseSensitivitySlider.value = sens;

        // Apply on start
        AudioManager.Instance.SetMasterVolume(master);
        AudioManager.Instance.SetMusicVolume(music);
        AudioManager.Instance.SetSFXVolume(sfx);
        //GameSettings.MouseSensitivity = sens;
    }
    */
}
