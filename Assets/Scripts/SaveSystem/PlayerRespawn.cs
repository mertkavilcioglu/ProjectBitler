using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    AudioManager audioManager;
    PlayerHealth playerHealth;

    private void Awake()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.HasKey("LastCheckpoint") && currentScene == "Ayasofya_ic")
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            transform.position = new Vector3(x, y, z);
            Debug.Log($"Player spawned at checkpoint: {transform.position}");
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            // Reset all data including mission data
            PlayerPrefs.DeleteAll();

            // Also reset mission manager data if it exists
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.ResetAllMissionData();
            }

            Debug.Log("ALL DATA RESET! Position and mission progress will be default on next spawn.");
        }
    }

    // Call this method when the player dies
    public void OnPlayerDeath()
    {
        // Make sure mission status is saved before respawning
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.SaveMissionStatus();
        }

        // Your existing death/respawn code...
    }
}