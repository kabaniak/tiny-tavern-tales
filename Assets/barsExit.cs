using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barsExit : MonoBehaviour
{
    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
