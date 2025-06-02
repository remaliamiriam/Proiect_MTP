using UnityEngine;

public class GYManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject youWonPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
        youWonPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ShowYouWon()
    {
        youWonPanel.SetActive(true);
        Time.timeScale = 0f;  
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
