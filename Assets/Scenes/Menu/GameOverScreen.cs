using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void Restart()
    {
        if (PlayerPrefs.HasKey("LastActiveScene"))
        {
            string levelToLoad = PlayerPrefs.GetString("LastActiveScene");
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}