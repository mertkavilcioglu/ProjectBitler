using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider; 

    private void Start()
    {
        if (audioMixer != null && volumeSlider != null)
        {
            float currentVolume;
            if (audioMixer.GetFloat("volume", out currentVolume))
            {
                volumeSlider.value = currentVolume;
            }
        }
    }

    public void SetVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("volume", volume);
            Debug.Log("Volume set to: " + volume);
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}