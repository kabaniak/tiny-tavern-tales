using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    GameObject tutorialText;
    GameObject tutorialBg;
    GameObject instructions;

    string[] script =
    {
        "Congratulations! You've just opened your Tiny Tavern",
        "Player 1 moves around with AWSD and interacts with E",
        "Player 2 moves around with the arrow keys and interacts with SLASH",
        "Why don't you all try moving around for a second?",
        "wait 5",
        "You both need to work together to keep your tavern running smoothly!",
        ""
    };

    int currPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        enableEverything(false);
        tutorialText = GameObject.FindGameObjectWithTag("Tutorial Text");
        tutorialBg = GameObject.FindGameObjectWithTag("Tutorial BG");
        instructions = GameObject.FindGameObjectWithTag("Instructions");

    }

    // Update is called once per frame
    void Update()
    {
        var moveOn = Input.GetKeyDown(KeyCode.Space);

        if (moveOn)
        {
            currPoint += 1;
            if(currPoint >= script.Length)
            {
                // WERE DONE WITH TUTORIAL
                return;
            }
            if (script[currPoint].Contains("wait"))
            {
                tutorialBg.SetActive(false);
                instructions.GetComponent<RectTransform>().localPosition = new Vector3(463,469,0);
                enablePlayers();
            }
            else
            {
                enableEverything(false);
                tutorialText.GetComponent<UnityEngine.UI.Text>().text = script[currPoint];
                tutorialBg.SetActive(true);

                instructions.GetComponent<RectTransform>().localPosition = new Vector3(-22, -242, 0);
            }
        }
    }

    void enablePlayers()
    {
        GameObject.FindObjectOfType<player1control>().enabled = true;
        GameObject.FindObjectOfType<player2control>().enabled = true;
    }

    void enableEverything( bool on)
    {
        // disable players
        GameObject.FindObjectOfType<player1control>().enabled = on;
        GameObject.FindObjectOfType<player2control>().enabled = on;

        // disable stations
        GameObject.FindObjectOfType<cookStation>().enabled = on;
        GameObject.FindObjectOfType<Kegs>().enabled = on;
        GameObject.FindObjectOfType<Table>().enabled = on;

        NPCSpriteBehavior[] npcs = GameObject.FindObjectsOfType<NPCSpriteBehavior>();
        foreach(NPCSpriteBehavior n in npcs) { n.enabled = on; }
    }


}
