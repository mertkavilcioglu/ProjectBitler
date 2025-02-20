using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public AudioClip MenuMusic;
    
    public void Restart()
    {
        SceneManager.LoadScene("Boss");
        audioSource.PlayOneShot(MenuMusic);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
        audioSource.PlayOneShot(MenuMusic);
    }
    
}
