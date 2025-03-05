using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void NewGame()
    {
        SceneManager.LoadScene("Map");
    }
    AudioManager audioManager;
    

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
    }
    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("LastActiveScene"))
        {
            string levelToLoad = PlayerPrefs.GetString("LastActiveScene");
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
