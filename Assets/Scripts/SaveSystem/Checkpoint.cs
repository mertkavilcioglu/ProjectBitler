using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    //public int checkpointID;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string activeScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("LevelSaved", activeScene);
            Debug.Log(activeScene);
            gameObject.SetActive(false);
            /*
            PlayerPrefs.SetInt("Last Checkpoint", checkpointID);
            PlayerPrefs.Save();
            
            Vector3 checkpointPos = transform.position;
            PlayerPrefs.SetFloat("CheckpointX", checkpointPos.x);
            PlayerPrefs.SetFloat("CheckpointY", checkpointPos.y);
            PlayerPrefs.SetFloat("CheckpointZ", checkpointPos.z);
            PlayerPrefs.Save();*/
        }
    }
}
