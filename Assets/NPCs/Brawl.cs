using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brawl : MonoBehaviour
{
    GameObject[] involvedNPCs = { null, null };

    bool p1Interacting = false;
    bool p2Interacting = false;

    float p1Timer = 0;
    float p2Timer = 0;

    float sensitivity = 0.3f;

    bool onPath = false;
    float radius = 7;
    Vector2 circleCenter;
    Vector2 startDir;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.y > 0) { circleCenter = new Vector2(1, 9); }
        else { circleCenter = new Vector2(1, -7); }

        startDir = new Vector2(circleCenter.x - transform.position.x, circleCenter.y - transform.position.y);
        if(startDir.magnitude < radius) { startDir.Scale(new Vector2(-1, -1)); }
        startDir.Normalize();

    }

    // Update is called once per frame
    void Update()
    {
        // check if players are interacting
        if (Input.GetKeyDown(KeyCode.E)) 
        { 
            p1Interacting = true;
            p1Timer = Time.time + sensitivity;
        }
        if (Input.GetKeyDown(KeyCode.Slash)) 
        { 
            p2Interacting = true;
            p2Timer = Time.time + sensitivity;
        }

        // see how long since they've held the key
        if(Time.time > p1Timer)
        {
            p1Interacting = false;
        }
        if (Time.time > p2Timer)
        {
            p2Interacting = false;
        }

        // make sure players are in range
        var p1Here = GameObject.FindObjectOfType<player1control>().inRangeBrawl;
        var p2Here = GameObject.FindObjectOfType<player2control>().inRangeBrawl;

        if (p1Here && p2Here)
        {
            if (p1Interacting && p2Interacting)
            {
                // end brawl if all break up conditions are met
                endBrawl();
            }
        }

        // if players leave, they can't break it up anymore
        else
        {
            if (!p1Here)
            {
                p1Interacting = false;
            }
            if (!p2Here)
            {
                p2Interacting = false;
            }
        }

    }

    private void FixedUpdate()
    {
        moveAround();
    }

    public void startBrawl(GameObject npc1, GameObject npc2)
    {
        involvedNPCs[0] = npc1;
        involvedNPCs[1] = npc2;

        foreach (GameObject o in involvedNPCs)
        {
            o.GetComponent<NPCSpriteBehavior>().enterBrawl();
        }
    }

    public void endBrawl()
    {
        float dif = -1;
        foreach (GameObject o in involvedNPCs)
        {
            o.GetComponent<NPCSpriteBehavior>().transform.position = new Vector3(transform.position.x + dif, transform.position.y, 0);
            o.GetComponent<NPCSpriteBehavior>().exitBrawl();
            dif += 2;
        }

        Destroy(gameObject);
    }

    public void moveAround()
    {
        if (!onPath)
        {
            Vector3 currPos = transform.position;
            transform.position = new Vector3(currPos.x + startDir.x * 0.08f, currPos.y + startDir.y * 0.08f, 0);

            float distFromCenter = (new Vector2(circleCenter.x - transform.position.x, circleCenter.y - transform.position.y)).magnitude;
            if(distFromCenter < radius + 0.1 && distFromCenter > radius - 0.1)
            {
                onPath = true;
            }
        }
        else
        {

        }
    }
}
