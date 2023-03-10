using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public int totalCoins = 0;
    public int reputation = 13;
    public int maxRating = 25;
    GameObject starBar, dayreportCard, finalReport;
    public GameObject generator, p1, p2, cookMask, gameOver, congrats, pause, dayCycle;

    public bool tutorial;

    GameObject cookStation, prepStation;

    public int brawlsToday, brawlsFinal, angryToday, angryFinal, servedToday, servedFinal, coinsToday = 0;
    public int dayCount = 1;
    private Vector3 startPos1;
    private Vector3 startPos2;

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
        Time.timeScale = 1f;
        textDisp = GameObject.FindGameObjectWithTag("Money Stat");
        starBar = GameObject.Find("StarRatingBar");
        generator = GameObject.Find("NPC Generator");
        dayCycle = GameObject.Find("DayCycleFill");
        p1 = GameObject.Find("p1");
        if (p1)
        {
            startPos1 = p1.transform.position;
        }
        p2 = GameObject.Find("p2");
        if (p2)
        {
            startPos2 = p2.transform.position;
        }
        cookMask = GameObject.Find("CookMask");
        cookStation = GameObject.FindWithTag("Heat");
        prepStation = GameObject.FindWithTag("Prep");

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
        dayreportCard = GameObject.Find("ReportCard");
        if (dayreportCard)
        {
            dayreportCard.SetActive(false);
        }
        finalReport = GameObject.Find("FinalReport");
        if (finalReport)
        {
            finalReport.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(reputation < 0) { reputation = 0; }
        if(reputation > maxRating) { reputation = maxRating; }
        if (!tutorial)
        {
            if (reputation == 0)
            {
                Apocalypse();
            }
            // Go to Next Day
            if (dayreportCard.activeSelf & Input.GetKeyDown(KeyCode.Space) & !finalReport.activeSelf)
            {
                NextDay();
            }
        }

        if (finalReport.activeSelf)
        {
            JustCalc();
        }
    }

    public void orderCompleted(int value, int effect)
    {
        coinsToday += value;
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
        generator.GetComponent<NPCGenerator>().generating = false;
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

    public void NextDay()
    {
        // reset stats
        dayCount += 1;
        dayCycle.GetComponent<Image>().fillAmount = 0f;
        brawlsFinal += brawlsToday;
        brawlsToday = 0;
        angryFinal += angryToday;
        angryToday = 0;
        servedFinal += servedToday;
        servedToday = 0;
        coinsToday = 0;

        resetPlayers();

        cookStation.GetComponent<cookStation>().resetCook();
        prepStation.GetComponent<prepStation>().resetPrep();

        clearPlates();

        dayreportCard.SetActive(false);
        Time.timeScale = 1f;
        dayCycle.GetComponent<dayCycle>().destroyedAlready = false;
        generator.GetComponent<NPCGenerator>().resetQueue();
        generator.GetComponent<NPCGenerator>().changeInterval(dayCount);
        generator.GetComponent<NPCGenerator>().generating = true;
    }

    // clear the plates
    public void clearPlates()
    {
        foreach(Table t in GameObject.FindObjectsOfType<Table>())
        {
            t.clearTable();
        }

        GameObject.Find("Counter1").GetComponent<holdingStationLogic>().clearCounter();
        GameObject.Find("Counter2").GetComponent<holdingStationLogic>().clearCounter();
        GameObject.Find("Counter3").GetComponent<holdingStationLogic>().clearCounter();
    }

    public void resetPlayers()
    {

        p1.transform.position = startPos1;
        p2.transform.position = startPos2;

        p1.GetComponent<player1control>().FeedtheDog();
        p2.GetComponent<player2control>().FeedtheDog();

        p1.GetComponent<player1control>().updateSpriteByCurrObject();
        p2.GetComponent<player2control>().updateSpriteByCurrObject();
    }

    public void JustCalc()
    {
        brawlsFinal += brawlsToday;
        brawlsToday = 0;
        angryFinal += angryToday;
        angryToday = 0;
        servedFinal += servedToday;
        servedToday = 0;
        coinsToday = 0;
    }
}
