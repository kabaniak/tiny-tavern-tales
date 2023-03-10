using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dayCycle : MonoBehaviour
{
    private float timeRate = 0.009f;
    private bool stop = false;
    GameObject dailyReport;
    GameObject finalReport;
    GameObject gameManager;
    GameObject NPCGenerator;
    public Text dailyInstructText;
    private float delay = 5f;
    private float placeTime = 0f;
    public string MainMenu;

    public bool tutorial;
    public bool destroyedAlready = false;

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

        if (gameObject.transform.GetComponent<Image>().fillAmount == 1 && !destroyedAlready)
        {
            NPCGenerator.GetComponent<NPCGenerator>().generating = false;
            // destroy any npcs waiting outside restaurant
            destroyWaiting();
            destroyedAlready = true;
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

        if (finalReport.activeSelf & Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("This works");
            SceneManager.LoadScene(0);
        }
    }

    void destroyWaiting()
    {
        foreach(GameObject npc in GameObject.FindGameObjectsWithTag("NPC")) 
        {
            NPCSpriteBehavior n = npc.GetComponent<NPCSpriteBehavior>();
            if(n && n.getCurrentState() == "waiting" && GameObject.FindObjectOfType<NPCGenerator>().getPlaceInQueue(n.id) >= 8)
            {
                n.currentState = "beDestroyed";
            }
        }
    }
}
