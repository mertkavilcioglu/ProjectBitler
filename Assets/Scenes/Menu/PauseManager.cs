using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool isPaused = false;
    private void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    private void Awake()
    {
        Canvas pauseMenuCanvas = pauseMenu.GetComponent<Canvas>();
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.sortingOrder = 10;
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Canvas healthBarCanvas = FindObjectOfType<PlayerHealth>()?.healthBarCanvas;
        if (healthBarCanvas != null)
        {
            healthBarCanvas.gameObject.SetActive(false);
        }
        Time.timeScale = 0;
        isPaused = true;
    }
    public void Home()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
        isPaused = false;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Canvas healthBarCanvas = FindObjectOfType<PlayerHealth>()?.healthBarCanvas;
        if (healthBarCanvas != null)
        {
            healthBarCanvas.gameObject.SetActive(true);
        }
        Time.timeScale = 1;
        isPaused = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        isPaused = false;
    }
}