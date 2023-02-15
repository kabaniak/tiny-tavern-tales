using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int totalCoins = 0;
    int reputation = 50;

    GameObject textDisp;

    public enum prices
    {
        correct = 5,
        incorrect = 0
    }

    public enum popularity
    {
        correct = 1,
        incorrect = -1
    }


    // Start is called before the first frame update
    void Start()
    {

        textDisp = GameObject.FindGameObjectWithTag("Money Stat");


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void orderCompleted(int value, int effect)
    {
        totalCoins += value;
        textDisp.GetComponent<UnityEngine.UI.Text>().text = totalCoins.ToString();
        
        reputation += effect;
    }
}
