using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brawl : MonoBehaviour
{
    GameObject[] involvedNPCs = { null, null };
    public GameObject messPrefab;

    bool p1Interacting = false;
    bool p2Interacting = false;

    float p1Timer = 0;
    float p2Timer = 0;

    float sensitivity = 0.3f;

    bool onPath = false;
    float radius = 7;
    Vector2 circleCenter;
    Vector2 startDir;
    bool clockwise;
    float currAngle;

    float nextDam;
    int sinceMess = 0;
    int messInt = 3;
    float interval = 2.5f;


    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.y > 0) { circleCenter = new Vector2(1, 9); }
        else { circleCenter = new Vector2(1, -7); }

        startDir = new Vector2(circleCenter.x - transform.position.x, circleCenter.y - transform.position.y);
        if(startDir.magnitude < radius) { startDir.Scale(new Vector2(-1, -1)); }
        startDir.Normalize();

        nextDam = Time.time + interval;

        if(Random.value < 0.5) { clockwise = true; }
        else { clockwise = false; }

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

        if (p1Here == gameObject && p2Here == gameObject)
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

        if(Time.time > nextDam)
        {
            if(sinceMess >= messInt)
            {
                createMess();
                sinceMess = 0;
            }
            GameObject.FindObjectOfType<GameManager>().updateRep(-1);

            if(Random.value < 0.2) { clockwise = !clockwise; }
            nextDam = Time.time + interval;
            sinceMess++;
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

                currAngle = Mathf.Atan2(currPos.y - circleCenter.y, currPos.x - circleCenter.x);
            }
        }
        else
        {
            if (clockwise) { currAngle -= 0.02f; }
            else { currAngle += 0.02f; }

            float newXPos = circleCenter.x + Mathf.Cos(currAngle) * radius;
            float newYPos = circleCenter.y + Mathf.Sin(currAngle) * radius;

            if(Random.value > 0.8)
            {
                newXPos += Random.Range(-.5f, 0.5f);
            }
            if(Random.value > 0.8)
            {
                newYPos += Random.Range(-.5f, 0.5f);
            }

            transform.position = new Vector3(newXPos, newYPos, 0);
        }
    }

    void createMess()
    {
        Vector2 fromCenter = new Vector2(transform.position.x - circleCenter.x, transform.position.y - circleCenter.y);
        fromCenter.Normalize();
        float xPos = transform.position.x + fromCenter.x * ( Random.Range(-0.5f, 10)) + Random.Range(-2f, 2f);
        float yPos = transform.position.y + fromCenter.y * ( Random.Range(-0.5f, 6)) + Random.Range(-2f, 2f);

        if (yPos < -15)
        {
            yPos = -15 + Random.Range(0, 2f) ;
        }
        else if (yPos > 15)
        {
            yPos = 15 + Random.Range(-2f, 0);
        }
        GameObject messNew = Instantiate(messPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
        messNew.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 0f, 0.6f, 0.9f, 1.0f);
    }
}
