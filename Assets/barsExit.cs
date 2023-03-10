using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class barsExit : MonoBehaviour
{
    private bool paused = false;
    public GameObject pauseMenu;
    public string MainMenu;
    
    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
    }
    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }

    public void PauseGame()
    {
        if (paused)
        {
            ResumeGame();
            paused = false;
            pauseMenu.SetActive(false);
        }
        else
        {
            paused = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(MainMenu);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
