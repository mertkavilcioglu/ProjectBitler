using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ResetAllMissionData()
    {
        PlayerPrefs.DeleteKey("Mission1Completed");
        PlayerPrefs.DeleteKey("Mission2Completed");
        PlayerPrefs.DeleteKey("Mission3Completed");
        PlayerPrefs.DeleteKey("CompletedMissions");
        PlayerPrefs.DeleteKey("CompletedAreas");
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("LastCheckpoint");
        PlayerPrefs.Save();

        Debug.Log("All mission data has been reset");
    }

    public void NewGame()
    {
        ResetAllMissionData();
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
