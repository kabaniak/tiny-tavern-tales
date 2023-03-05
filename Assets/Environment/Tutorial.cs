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
        "waitt",
        "You both need to work together to keep your tavern running smoothly!",
        "Let's welcome our first customer",
        "createNPC beer",
        "Once they sit down, the customer's order appears on the order tracker (to the right)",
        "The order shows the customer's face next to the item they want",
        "This customer wants a mug of ale which you can find on the top left",
        "Why don't you go deliver it to them now?",
        "action serve beer",
        "waitt set",
        "Awesome! Look at that happy customer go :)",
        "When customers get the order they want, they'll pay you coins",
        "You can see how many you've earned in the top left",
        "Let's welcome in another customer!",
        "createNPC meat",
        "This customer wants some hearty meat - why don't we learn how to prepare it?",
        "First we need to prep the meat",
        "You can get raw meat from the cellar on the bottom left",
        "To prep, bring it to the prep station on the bottom right and tap to cut",
        "Give it a try!",
        "action prep",
        "Amazing prepping! Now it's ready to be cooked",
        "Take the prepped meat to the stove on the top right and wait until it's done",
        "Make sure you pick it up after the bar turns green or it will start to burn!",
        "Don't worry though, you can feed any unwanted food to the dog",
        "action cook",
        "Perfect job!",
        "Why don't you go deliver it to the customer?",
        "action serve meat",
        "Great!"
    };

    int currPoint = 0;

    bool waiting = false;
    public delegate bool WaitCondition();
    WaitCondition cond;

    // timer cond
    float waitUntil;

    // npc serving conds
    string npcDesiredState;
    bool angerState;

    // item prep conds
    string waitItem;


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
                enablePlayers(false);
                tutorialText.GetComponent<UnityEngine.UI.Text>().text = script[currPoint];
                tutorialBg.SetActive(true);
                instructions.GetComponent<RectTransform>().localPosition = new Vector3(-22, -242, 0);
                instructions.SetActive(true);
            }
        }
    }

    bool checkIfCommand()
    {
        if (script[currPoint].Contains("waitt")) {
            hideTutorial();
            enablePlayers(true);
            if (script[currPoint].Contains("set"))
            {
                setWaitFor(6.25f);
                hideInstructions();
            }
            return true;
        }
        else if(script[currPoint].Contains("createNPC"))
        {

            for (int i = npcList.Count - 1; i >= 0; i--)
            {
                if (npcList[i] == null) { npcList.RemoveAt(i); }
            }
            hideTutorial();
            hideInstructions();
            enableNPCThings(true);
            npcList.Add(Instantiate(NPCPrefab, new Vector3(-10, -20, 0), Quaternion.identity));
            controlNPCPatience(0);

            if (script[currPoint].Contains("beer"))
            {
                npcList[0].GetComponent<NPCSpriteBehavior>().setOrder(true);
            }
            else
            {
                npcList[0].GetComponent<NPCSpriteBehavior>().setOrder(false);
            }

            setWaitFor(4.25f);
            return true;
        }
        else if (script[currPoint].Contains("action"))
        {
            hideTutorial();
            hideInstructions();
            enablePlayers(true);
            enableNPCThings(false);

            kegs.SetActive(false);

            string command = script[currPoint].Substring(7);
            if(command.Contains("serve"))
            {

                enableNPCThings(true);
                if (command.Contains("beer"))
                {
                    kegs.SetActive(true);
                }
                if (command.Contains("meat"))
                {
                    meatRack.SetActive(true);
                    prep.SetActive(true);
                    cook.SetActive(true);
                }

                setNPCState("eating", false);
            }
            else if (command.Contains("prep"))
            {
                meatRack.SetActive(true);
                prep.SetActive(true);
                setWaitItem("PreppedMeat");
            }
            else if (command.Contains("cook"))
            {
                meatRack.SetActive(true);
                prep.SetActive(true);
                cook.SetActive(true);
                setWaitItem("CookedMeat");
            }

            return true;
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

    void setNPCState(string desiredState, bool anger)
    {
        waiting = true;
        npcDesiredState = desiredState;
        angerState = anger;
        cond = npcCondMet;
    }

    bool npcCondMet()
    {
        NPCSpriteBehavior npc = npcList[0].GetComponent<NPCSpriteBehavior>();
        if (npc.getCurrentState() == npcDesiredState && npc.isAngry() == angerState) { return true; }

        else if (npc.getCurrentState() == npcDesiredState && npc.isAngry() != angerState)
        {
            script[currPoint - 1] = "Oops! That wasn't quite right. Why don't you try again?";
            currPoint -= 1;
            return true;
        }
        return false;
    }

    void setWaitItem(string item)
    {
        waiting = true; 
        waitItem = item;
        cond = gotItem;
    }

    bool gotItem()
    {
        if(GameObject.FindObjectOfType<player1control>().currentObject == "BurntMeat" || GameObject.FindObjectOfType<player2control>().currentObject == "BurntMeat")
        {
            script[currPoint - 1] = "Oops! Look like you burned it. Feed it to the dog and start over";
            currPoint-=2;
            return true;
        }
        return GameObject.FindObjectOfType<player1control>().currentObject == waitItem || GameObject.FindObjectOfType<player2control>().currentObject == waitItem;
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

    void enablePlayers(bool on)
    {
        GameObject.FindObjectOfType<player1control>().enabled = on;
        GameObject.FindObjectOfType<player2control>().enabled = on;
    }

    void enableNPCThings(bool on)
    {
        GameObject.FindObjectOfType<Table>().enabled = on;
        NPCSpriteBehavior[] npcs = GameObject.FindObjectsOfType<NPCSpriteBehavior>();
        foreach (NPCSpriteBehavior n in npcs) { n.enabled = on; }
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
