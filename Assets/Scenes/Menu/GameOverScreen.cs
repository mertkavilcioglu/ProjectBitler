using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Map");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}