using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dayCycle : MonoBehaviour
{
    // private float timeRate = 0.005f;
    //temp
    private bool stop = false;
    private float timeRate = 0.1f;
    GameObject dailyReport;
    GameObject finalReport;
    GameObject gameManager;
    GameObject NPCGenerator;
    public Text dailyInstructText;

    public bool tutorial;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetComponent<Image>().fillAmount = 0f;
        gameManager = GameObject.Find("GManager");
        finalReport = GameObject.Find("FinalReport");
        dailyReport = GameObject.Find("ReportCard");
        NPCGenerator = GameObject.Find("NPC Generator");
        dailyInstructText = GameObject.Find("RCInstructionText").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {  
        if (gameObject.transform.GetComponent<Image>().fillAmount <= 1f)
        {
            gameObject.transform.GetComponent<Image>().fillAmount += timeRate * Time.deltaTime;
        }
    }

    void Update()
    {
        var npcCount = GameObject.FindGameObjectsWithTag("NPC").Length;

        var brawlCount = GameObject.FindGameObjectsWithTag("Brawl").Length;

        if (gameObject.transform.GetComponent<Image>().fillAmount == 1)
        {
            NPCGenerator.SetActive(false);
        }

        if (tutorial) { return; }

        if (gameObject.transform.GetComponent<Image>().fillAmount == 1 &
            gameManager.GetComponent<GameManager>().dayCount < 3 &
            npcCount == 0 &
            brawlCount == 0)
        {
            dailyReport.SetActive(true);
            Time.timeScale = 0f;
        }

        if (gameManager.GetComponent<GameManager>().dayCount == 3)
        {
            dailyInstructText.text = "Press SPACE to go see your final report";
        }


        if (gameObject.transform.GetComponent<Image>().fillAmount == 1 &
            gameManager.GetComponent<GameManager>().dayCount == 3 & 
            npcCount == 0 &
            brawlCount == 0 &
            stop == false)
        {
            dailyReport.SetActive(true);
            stop = true;
            Time.timeScale = 0f;
        }

        if (gameObject.transform.GetComponent<Image>().fillAmount == 1 &
            gameManager.GetComponent<GameManager>().dayCount == 3 &
            npcCount == 0 &
            brawlCount == 0 &
            dailyReport.activeSelf &
            stop == true & Input.GetKeyDown(KeyCode.Space))
        {
            finalReport.SetActive(true);
        }
    }
}
