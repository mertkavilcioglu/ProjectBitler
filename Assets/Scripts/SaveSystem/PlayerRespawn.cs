using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    void Start()
    {
        RespawnAtLastCheckpoint();
    }

    public void RespawnAtLastCheckpoint()
    {
        if (PlayerPrefs.HasKey("LastCheckpoint"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");

            transform.position = new Vector3(x, y, z);
        }
        else { Debug.Log("No checkpoint, spawning default."); }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("RESETTED! ** Pressing R resets the checkpoint.");
        }

    }
}
