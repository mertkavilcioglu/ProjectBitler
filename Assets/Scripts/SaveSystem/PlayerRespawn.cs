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
        playerHealth = GetComponent<PlayerHealth>();

    }
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.HasKey("LastCheckpoint") && currentScene != "Ayasofya_ic")
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            transform.position = new Vector3(x, y, z);
            Debug.Log($"Player spawned at checkpoint: {transform.position}");
        }
        /////////////////////////////////////////
        if (currentScene == "Ayasofya_ic" && playerHealth != null)
        {
            playerHealth.healthBarOffset = new Vector3(-5.7f, 15.3f, 0f);
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            // Reset all data including mission data
            PlayerPrefs.DeleteAll();
            Debug.Log("CHECKPOINT DATA RESET! Position will be default on next spawn.");

            // Also reset mission manager data if it exists
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.ResetAllMissionData();
            }

            Debug.Log("ALL DATA RESET! Position and mission progress will be default on next spawn.");
        }
    }

    public void OnPlayerDeath()
    {
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.SaveMissionStatus();
        }

    }
}
