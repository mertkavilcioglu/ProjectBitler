using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Boss");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}