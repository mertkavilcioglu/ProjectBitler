using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenuManager : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(MenuMusic);
    }
    public AudioClip MenuMusic;
    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        
        audioMixer.SetFloat("volume", volume);
        Debug.Log(volume);
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
