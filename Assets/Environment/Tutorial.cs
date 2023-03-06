using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public string mainMenu;

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
        "Customers that order ale pay 1 coin.",
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
        "waitt set",
        "Great! They loved it",
        "Since this customer ordered meat, they paid 3 coins - triple the price of ale",
        "You want to try to rack up as many coins as you can!",
        "Customers won't wait forever to get their order though",
        "Let's welcome two new customers and see what happens when we make them wait too long",
        "createNPC meat 2",
        "makeNPCmad",
        "They look MAD, NPCs will get redder the longer you make them wait",
        "Once bright red they might leave ... or ...",
        "startBrawl",
        "Oh no! Angry NPCs are able to start brawls",
        "Look at the mess they're making in your tavern!",
        "To break up a brawl, both of you need to go and interact with it",
        "Hurry before it gets worse!",
        "action brawl",
        "waitt set",
        "Whew! That was stressful",
        "That brawl left behind a ton of messes",
        "Go clean them up or they'll bother your other customers",
        "action clean",
        "Awesome job you two!",
        "There a just a few more things",
        "enableDay",
        "On the top right is a bar indicating how far in the day you've progressed",
        "Once it's filled you won't get any more customers, but you can finish serving anyone already there",
        "You get three days to run your tavern as best you can",
        "At the end of the day you'll get a report card showing how well you did",
        "The other two buttons on the top right are to pause the game and quit to the menu",
        "Last thing you need to know is to keep track of your rating",
        "Your rating is shown in the stars at the top of the screen",
        "Serving orders correctly will increase your rating",
        "Serving them wrong, making customers wait too long, and brawling will all decrease it",
        "If you let your rating get to no stars the tavern will get shut down :(",
        "Good luck your tavern is ready to open for business now!",
        "Hit SPACE to return to the main menu"
    };

    int currPoint = -1;

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
    bool tempReset = false;
    float tempTimer;


    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<GameManager>().tutorial = true;
        GameObject.FindObjectOfType<NPCGenerator>().tutorial = true;
        GameObject.FindObjectOfType<dayCycle>().tutorial = true;

        kegs = GameObject.FindGameObjectWithTag("Kegs");
        meatRack = GameObject.FindGameObjectWithTag("MeatRack");
        prep = GameObject.FindGameObjectWithTag("Prep");
        cook = GameObject.FindGameObjectWithTag("Heat");

        //enableEverything(false);
        tutorialText = GameObject.FindGameObjectWithTag("Tutorial Text");
        tutorialBg = GameObject.FindGameObjectWithTag("Tutorial BG");
        instructions = GameObject.FindGameObjectWithTag("Instructions");

    }

    // Update is called once per frame
    void Update()
    {
        var moveOn = Input.GetKeyDown(KeyCode.Space);

        if (tempReset && tempTimer < Time.time) 
        {
            tempReset = false;
        }

        if(currPoint == -1)
        {
            enableEverything(false);
            currPoint += 1;
            tutorialText.GetComponent<UnityEngine.UI.Text>().text = script[currPoint];
            tutorialBg.SetActive(true);
            instructions.GetComponent<RectTransform>().localPosition = new Vector3(-22, -242, 0);
            instructions.SetActive(true);
        }

        if ((!waiting && moveOn) || (waiting && cond()))
        {
            waiting = false;
            currPoint += 1;
            if(currPoint >= script.Length)
            {
                // WERE DONE WITH TUTORIAL
                SceneManager.LoadScene(mainMenu);
                return;
            }
            if (! checkIfCommand())
            {
                enablePlayers(false);
                enableNPCThings(false);
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
                enablePlayers(false);
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
            int id = 0;
            if (script[currPoint].Contains("2"))
            {
                npcList.Add(Instantiate(NPCPrefab, new Vector3(-10, -20, 0), Quaternion.identity));
                npcList[0].GetComponent<NPCSpriteBehavior>().setId(id);
                id += 1;
                controlNPCPatience(0);
            }
            npcList.Add(Instantiate(NPCPrefab, new Vector3(-10, -20, 0), Quaternion.identity));
            npcList[0].GetComponent<NPCSpriteBehavior>().setId(id);
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
            else if (command.Contains("brawl"))
            {
                enableNPCThings(true);
                enablePlayers(true);
                setWaitBrawl();
            }
            else if (command.Contains("clean"))
            {
                enableNPCThings(true);
                enablePlayers(true);
                setWaitMess();
            }

            return true;
        }
        else if (script[currPoint].Contains("makeNPCmad"))
        {
            hideTutorial();
            hideInstructions();
            enablePlayers(false);
            enableNPCThings(true);
            controlNPCPatience(1);
            setWaitFor(4.5f);
            return true;
        }
        else if (script[currPoint].Contains("startBrawl"))
        {
            hideTutorial();
            hideInstructions();
            enablePlayers(false);
            enableNPCThings(true);
            setWaitFor(3f);
            return true;
        }
        else if(script[currPoint].Contains("enableDay"))
        {
            GameObject.FindObjectOfType<dayCycle>().enabled = true;
            currPoint += 1;
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
            currPoint -= 2;
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
        if(!tempReset && (GameObject.FindObjectOfType<player1control>().currentObject == "BurntMeat" || GameObject.FindObjectOfType<player2control>().currentObject == "BurntMeat"))
        {
            script[currPoint - 1] = "Oops! Look like you burned it. Feed it to the dog and start over";
            currPoint-=2;
            tempReset = true;
            tempTimer = Time.time + 7f;
            return true;
        }
        return GameObject.FindObjectOfType<player1control>().currentObject == waitItem || GameObject.FindObjectOfType<player2control>().currentObject == waitItem;
    }


    void setWaitBrawl()
    {
        waiting = true;
        cond = noBrawls;
    }

    bool noBrawls()
    {
        return GameObject.FindGameObjectWithTag("Brawl") == null;
    }

    void setWaitMess()
    {
        waiting = true;
        cond = noMess;
    }

    bool noMess()
    {
        return GameObject.FindGameObjectWithTag("Mess") == null;
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
        if (state == 1)
        {
            foreach (GameObject npc in npcList)
            {
                npc.GetComponent<NPCSpriteBehavior>().pausePatience(false);
                npc.GetComponent<NPCSpriteBehavior>().setBrawlChance(2.0f);
                npc.GetComponent<NPCSpriteBehavior>().setPatience(5.0f);
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
        enableTable(on);
        NPCSpriteBehavior[] npcs = GameObject.FindObjectsOfType<NPCSpriteBehavior>();
        foreach (NPCSpriteBehavior n in npcs) { n.enabled = on; }
    }

    void enableTable(bool on)
    {
        GameObject.FindObjectOfType<Table>().enabled = on;
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

        GameObject.FindObjectOfType<dayCycle>().enabled = on;


        NPCSpriteBehavior[] npcs = GameObject.FindObjectsOfType<NPCSpriteBehavior>();
        foreach(NPCSpriteBehavior n in npcs) { n.enabled = on; }
    }


}
