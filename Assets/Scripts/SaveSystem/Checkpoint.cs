using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("LastCheckpoint", checkpointID);
            
            Vector3 checkpointPos = transform.position;
            PlayerPrefs.SetFloat("CheckpointX", checkpointPos.x);
            PlayerPrefs.SetFloat("CheckpointY", checkpointPos.y);
            PlayerPrefs.SetFloat("CheckpointZ", checkpointPos.z);
            PlayerPrefs.Save();
            Debug.Log($"Checkpoint {checkpointID} saved at position: {checkpointPos}");

            GetComponent<Collider2D>().enabled = false; // gameobjecti silmek yerine collider disable ediyo degistirilebilir
        }
    }
}
