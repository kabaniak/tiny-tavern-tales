using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int totalCoins = 0;
    public int reputation = 13;
    public int maxRating = 25;
    GameObject starBar;
    public GameObject generator, p1, p2, cookMask, gameOver, congrats, pause;

    public int brawlsToday, brawlsFinal, angryToday, angryFinal, servedToday, servedFinal, coinsToday = 0;

    GameObject textDisp;

    public enum prices
    {
        meat_correct = 3,
        booze_correct = 1,
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
        starBar = GameObject.Find("StarRatingBar");
        generator = GameObject.Find("NPC Generator");
        p1 = GameObject.Find("p1");
        p2 = GameObject.Find("p2");
        cookMask = GameObject.Find("CookMask");
        gameOver = GameObject.Find("GameOver");
        if (gameOver)
        {
            gameOver.SetActive(false);
        }
        congrats = GameObject.Find("Congrats");
        if (congrats)
        {
            congrats.SetActive(false);
        }
        pause = GameObject.Find("PauseMenu");
        if (pause)
        {
            pause.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(reputation < 0) { reputation = 0; }
        if(reputation > maxRating) { reputation = maxRating; }
        if (reputation == 0)
        {
            Apocalypse();
        }
    }

    public void orderCompleted(int value, int effect)
    {
        totalCoins += value;
        textDisp.GetComponent<UnityEngine.UI.Text>().text = totalCoins.ToString();
        
        reputation += effect;
    }

    public void updateRep(int effect)
    {
        reputation += effect;
    }

    private void Apocalypse()
    {
        generator.SetActive(false);
        p1.SetActive(false);
        p2.SetActive(false);
        cookMask.SetActive(false);
        GameObject[] brawls = (GameObject.FindGameObjectsWithTag("Brawl"));
        foreach (GameObject brawl in brawls)
            GameObject.Destroy(brawl);
        GameObject[] npcs = (GameObject.FindGameObjectsWithTag("NPC"));
        foreach (GameObject npc in npcs)
            GameObject.Destroy(npc);
        gameOver.SetActive(true);
    }
}
