using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject mainMenuUI; 
    public GameObject inGameUI;

    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        pauseMenuUI.SetActive(false);
        inGameUI.SetActive(false);
        mainMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}
