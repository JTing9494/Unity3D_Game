using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class OptionsWindow : Windows<OptionsWindow>
{
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] UnityEvent toggleWindowSound = null;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the window is already open, close it; otherwise, open it
            if (isOpen)
            {
                Close();
                toggleWindowSound.Invoke();
                // If in the Menu, do not lock the cursor
                if(SceneManager.GetActiveScene().buildIndex == 0)
                {
                    // Do not lock the cursor
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    // Lock the cursor in the game
                    Cursor.lockState = CursorLockMode.Locked;
                }
                Time.timeScale = 1f;
            }
            else
            {
                Open();
                toggleWindowSound.Invoke();
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0.001f;
            }
        }
    }

    [SerializeField] Slider masterVolumeSlider = null;
    [SerializeField] Slider bgmVolumeSlider = null;
    [SerializeField] Slider sfxVolumeSlider = null;
    [SerializeField] Text masterVolumeText = null;
    [SerializeField] Text bgmVolumeText = null;
    [SerializeField] Text sfxVolumeText = null;

    protected override void Start()
    {
        base.Start();

        // Retrieve volume settings from PlayerPrefs
        float masterVolume = PlayerPrefs.GetFloat("MASTER", 0);
        float bgmVolume = PlayerPrefs.GetFloat("BG", 0);
        float sfxVolume = PlayerPrefs.GetFloat("FX", 0);
        // Update slider values
        masterVolumeSlider.value = masterVolume;
        bgmVolumeSlider.value = bgmVolume;
        sfxVolumeSlider.value = sfxVolume;
        // Update volume display
        masterVolumeText.text = masterVolume.ToString("F1") + "db";
        bgmVolumeText.text = bgmVolume.ToString("F1") + "db";
        sfxVolumeText.text = sfxVolume.ToString("F1") + "db";

        UpdateAudioMixer();
    }

    public void OnValueChanged()
    {
        // Retrieve values from sliders
        float masterVolume = masterVolumeSlider.value;
        float bgmVolume = bgmVolumeSlider.value;
        float sfxVolume = sfxVolumeSlider.value;
        // Update volume display
        masterVolumeText.text = masterVolume.ToString("F1") + "db";
        bgmVolumeText.text = bgmVolume.ToString("F1") + "db";
        sfxVolumeText.text = sfxVolume.ToString("F1") + "db";
        // Save settings
        PlayerPrefs.SetFloat("MASTER", masterVolume);
        PlayerPrefs.SetFloat("BG", bgmVolume);
        PlayerPrefs.SetFloat("FX", sfxVolume);

        UpdateAudioMixer();
    }

    public void Reset()
    {
        masterVolumeSlider.value = 0f;
        bgmVolumeSlider.value = 0f;
        sfxVolumeSlider.value = 0f;
    }

    void UpdateAudioMixer()
    {
        audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        audioMixer.SetFloat("BGMVolume", bgmVolumeSlider.value);
        audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
    }

    public void ReturnToMainMenu()
    {
        ShadowManager.ins.Out(SwitchSceneToMainMenu);
    }

    void SwitchSceneToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
