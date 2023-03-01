using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    GameObject tutorialText;
    GameObject tutorialBg;
    GameObject instructions;

    public GameObject NPCPrefab;

    List<GameObject> npcList = new List<GameObject>();

    // stations
    GameObject kegs;
    GameObject meatRack;
    GameObject cook;
    GameObject prep;

    string[] script =
    {
        "Congratulations! You've just opened your Tiny Tavern",
        "Player 1 moves around with AWSD and interacts with E",
        "Player 2 moves around with the arrow keys and interacts with SLASH",
        "Why don't you all try moving around for a second?",
        "wait",
        "You both need to work together to keep your tavern running smoothly!",
        "Let's welcome our first customer",
        "createNPC",
        "Once they sit down, the customer's order appears on the order tracker (to the right)",
        "The order shows the customer's face next to the item they want",
        "This customer wants a mug of ale which you can find on the top left",
        "Why don't you go deliver it to them now?",
        "action serve beer"
    };

    int currPoint = 0;

    bool waiting = false;
    public delegate bool WaitCondition();
    WaitCondition cond;
    float waitUntil;

    // Start is called before the first frame update
    void Start()
    {
        kegs = GameObject.FindGameObjectWithTag("Kegs");
        meatRack = GameObject.FindGameObjectWithTag("MeatRack");
        prep = GameObject.FindGameObjectWithTag("Prep");
        cook = GameObject.FindGameObjectWithTag("Heat");

        enableEverything(false);
        tutorialText = GameObject.FindGameObjectWithTag("Tutorial Text");
        tutorialBg = GameObject.FindGameObjectWithTag("Tutorial BG");
        instructions = GameObject.FindGameObjectWithTag("Instructions");

    }

    // Update is called once per frame
    void Update()
    {
        var moveOn = Input.GetKeyDown(KeyCode.Space);

        if ((!waiting && moveOn) || (waiting && cond()))
        {
            waiting = false;
            currPoint += 1;
            if(currPoint >= script.Length)
            {
                // WERE DONE WITH TUTORIAL
                return;
            }
            if (! checkIfCommand())
            {
                enableEverything(false);
                tutorialText.GetComponent<UnityEngine.UI.Text>().text = script[currPoint];
                tutorialBg.SetActive(true);
                instructions.GetComponent<RectTransform>().localPosition = new Vector3(-22, -242, 0);
                instructions.SetActive(true);
            }
        }
    }

    bool checkIfCommand()
    {
        if (script[currPoint].Contains("wait")) {
            hideTutorial();
            enablePlayers();
            return true;
        }
        else if(script[currPoint] == "createNPC")
        {
            hideTutorial();
            hideInstructions();
            enableNPCThings();
            npcList.Add(Instantiate(NPCPrefab, new Vector3(-10, -20, 0), Quaternion.identity));
            controlNPCPatience(0);

            setWaitFor(4.25f);
            return true;
        }
        else if (script[currPoint].Contains("action "))
        {
            hideTutorial();
            enablePlayers();

            string command = script[currPoint].Substring(7);
            if(command.Contains("serve"))
            {
                if (command.Contains("beer"))
                {
                    kegs.SetActive(true);
                }
                if (command.Contains("meat"))
                {
                    enableEverything(true);
                }
            }
            else if (command.Contains("prep"))
            {
                meatRack.SetActive(true);
                prep.SetActive(true);
            }
            else if (command.Contains("cook"))
            {
                meatRack.SetActive(true);
                prep.SetActive(true);
                cook.SetActive(true);
            }
        }

        return false;
    }

    void setWaitFor(float time)
    {
        waiting = true;
        waitUntil = Time.time + time;
        cond = waitTimeDone;
    }

    bool waitTimeDone()
    {
        return Time.time > waitUntil;
    }

    void controlNPCPatience(int state)
    {
        if (state == 0)
        {
            foreach (GameObject npc in npcList)
            {
                npc.GetComponent<NPCSpriteBehavior>().pausePatience(true);
            }
        }
    }

    void hideTutorial()
    {
        tutorialBg.SetActive(false);
        instructions.SetActive(true);
        instructions.GetComponent<RectTransform>().localPosition = new Vector3(463, 469, 0);
    }

    void hideInstructions()
    {
        instructions.SetActive(false);
    }

    void enablePlayers()
    {
        GameObject.FindObjectOfType<player1control>().enabled = true;
        GameObject.FindObjectOfType<player2control>().enabled = true;
    }

    void enableNPCThings()
    {
        GameObject.FindObjectOfType<Table>().enabled = true;
        NPCSpriteBehavior[] npcs = GameObject.FindObjectsOfType<NPCSpriteBehavior>();
        foreach (NPCSpriteBehavior n in npcs) { n.enabled = true; }
    }

    void enableEverything( bool on)
    {
        // disable players
        GameObject.FindObjectOfType<player1control>().enabled = on;
        GameObject.FindObjectOfType<player2control>().enabled = on;

        // disable table script
        GameObject.FindObjectOfType<Table>().enabled = on;

        // disappear stations
        kegs.SetActive(on);
        meatRack.SetActive(on);
        cook.SetActive(on);
        prep.SetActive(on);


        NPCSpriteBehavior[] npcs = GameObject.FindObjectsOfType<NPCSpriteBehavior>();
        foreach(NPCSpriteBehavior n in npcs) { n.enabled = on; }
    }


}
