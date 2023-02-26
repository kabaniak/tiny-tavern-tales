using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int totalCoins = 0;
    public int reputation = 13;
    public int maxRating = 25;

    GameObject textDisp;
    public enum prices
    {
        meat_correct = 6,
        booze_correct = 3,
        incorrect = 0
    }

    public enum popularity
    {
        correct = 1,
        incorrect = -1,
        leave = -3,
        fight = -5
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

    public void noPatience(int effect)
    {
        reputation += effect;
    }
}
