using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject inGameUI;

    void Start()
    {
       
        mainMenuUI.SetActive(true);
        inGameUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        inGameUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}
