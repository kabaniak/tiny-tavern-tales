using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public string newGame;
    public string Tutorial2;
    public string MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene(newGame);
    }

    public void LoadTutorial2()
    {
        SceneManager.LoadScene(Tutorial2);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
