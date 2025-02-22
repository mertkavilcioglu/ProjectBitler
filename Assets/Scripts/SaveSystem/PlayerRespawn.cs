using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("LastCheckpoint"))
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
            PlayerPrefs.DeleteAll();
            Debug.Log("CHECKPOINT DATA RESET! Position will be default on next spawn.");
        }
    }
}
