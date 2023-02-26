using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteBehavior : MonoBehaviour
{
    SpriteRenderer spriteRender;

    // this will probably deprecate later
    Color orig;

    // information about the npc's stats
    private string myOrder = ""; // their order
    public float timeRemaining; // how long they'll wait
    private float speed = 0.1f; // how fast they move
    private bool angry = false;
    public float patience; // how willing they are to wait
    private float tolerance; // how quickly they lose patience
    

    // string representing the npc's state
    // either: waiting, toSeat, reachedSeat, ordered, eating, leaving
    private string currentState = "waiting";

    // information about where the npc is seated
    public Vector3 seatCoords;
    public GameObject mytable;

    private string NPCtype;
    private int haveReached = 0;
    private float sensitivity = 0.3f;
    private bool forward = true;
    private Vector2[] path = { new Vector2(-10,-20), new Vector2(0,0), new Vector2(0, 0), new Vector2(0, 0)};

    // temporary timer info
    public float timerStart = 0;

    //NPC Sprites
    public List<Sprite> HumanSprites;
    public List<Sprite> TieflingSprites;
    public List<Sprite> ElfSprites;

    // Start is called before the first frame update
    void Start()
    {
        // the amount of time this npc will wait for
        timeRemaining = 40;
        spriteRender = GetComponent<SpriteRenderer>();

        //choose NPC type
        int typenum = Random.Range(0, 3);
        if (typenum == 0)
        {
            NPCtype = "HumanFighter";
            tolerance = 1.0f;
            patience = 30f;
        }
        else if (typenum == 1)
        {
            NPCtype = "TieflingSorcerer";
            tolerance = 0.2f;
            patience = 90f;
        }
        else if (typenum == 2)
        {
            NPCtype = "ElfRogue";
            tolerance = 0.5f;
            patience = 70f;
        }
        //orig = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 0.7f);
        //spriteRender.color = orig;
        //Choose NPC sprite
        int spritenum = Random.Range(0, 6);
        if (NPCtype == "HumanFighter")
        {
            spriteRender.sprite = HumanSprites[spritenum];
        }
        else if (NPCtype == "TieflingSorcerer")
        {
            spriteRender.sprite = TieflingSprites[spritenum];
        }
        else if (NPCtype == "ElfRogue")
        {
            spriteRender.sprite = ElfSprites[spritenum];
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check if we're ready to be destroyed
        if(currentState == "beDestroyed")
        {
            Destroy(gameObject);
        }

        // if it's reached it's seat
        // and if we've been served our food
        else if (currentState == "eating")
        {
            if (angry)
            {
                spriteRender.color = Color.Lerp(new Color(1, 0, 0, 1.75f), orig, (timerStart - Time.time) / 2f);
            }

            // if timer has run out it should leave
            if ((timerStart + 2.0) < Time.time && currentState != "leaving")
            {
                leaveTable();
                currentState = "leaving";
            }
        }

        else if (currentState == "reachedSeat")
        {
            OrderTracker ot = GameObject.FindObjectOfType<OrderTracker>();
            ot.addOrder(gameObject);
            currentState = "ordered";
        }

        else if (currentState == "ordered")
        {
            patience -= tolerance * Time.deltaTime;
            if (patience <= 0)
            {
                angry = true;
                leaveTable();
                GameObject.FindObjectOfType<OrderTracker>().removeMyOrder(gameObject);
                currentState = "leaving";
            }
        }

        // if not seated yet
        else if (currentState == "waiting")
        {
            // try to find a table to go to
            GameObject[] tables = GameObject.FindGameObjectsWithTag("table");

            foreach(GameObject t in tables)
            {
                if (t.GetComponent<Table>().seatNPC(gameObject))
                {
                    mytable = t;
                    currentState = "toSeat";
                    seatCoords = t.GetComponent<Table>().getSeatCoords(gameObject);
                    int seatInd = t.GetComponent<Table>().getSeatInd(gameObject);

                    // record the path we should take
                    path[3] = new Vector2(seatCoords.x, seatCoords.y);

                    if(seatInd == 2)
                    {
                        path[1] = new Vector2(-10, 0);
                        path[2] = new Vector2(seatCoords.x, 0);
                    }
                    else
                    {
                        path[1] = new Vector2(-10, seatCoords.y);
                        path[2] = new Vector2(seatCoords.x, seatCoords.y);
                    }
                    break;
                }
            }

            // if no table to go to
            // pace up and down
        }
    }

    private void FixedUpdate()
    {
        // check if we're on our way out
        if (currentState == "leaving")
        {
            forward = false;
            move();
        }
        // if not at table yet
        else if (currentState == "toSeat")
        {
            forward = true;
            move();
        }
    }

    void move()
    {
        if(currentState == "leaving" || currentState == "toSeat")
        {
            // we are actively moving
            if (forward) 
            {
                // go to NEXT ind
                Vector3 currPos = transform.position;
                Vector2 dir = new Vector2(path[haveReached + 1].x - currPos.x, path[haveReached + 1].y - currPos.y);
                dir.Normalize();

                transform.position = new Vector3(currPos.x + dir.x * speed, currPos.y + dir.y * speed, 0);

                if (transform.position.x - sensitivity < path[haveReached + 1].x && transform.position.x + sensitivity > path[haveReached + 1].x)
                {
                    if (transform.position.y - sensitivity < path[haveReached + 1].y && transform.position.y + sensitivity > path[haveReached + 1].y)
                    {
                        haveReached = haveReached + 1;
                    }
                }
            }
            else
            {
                // go to PREV ind
                Vector3 currPos = transform.position;
                Vector2 dir = new Vector2(path[haveReached - 1].x - currPos.x, path[haveReached - 1].y - currPos.y);
                dir.Normalize();

                transform.position = new Vector3(currPos.x + dir.x * speed, currPos.y + dir.y * speed, 0);

                if (transform.position.x - sensitivity < path[haveReached - 1].x && transform.position.x + sensitivity > path[haveReached - 1].x)
                {
                    if (transform.position.y - sensitivity < path[haveReached - 1].y && transform.position.y + sensitivity > path[haveReached - 1].y)
                    {
                        haveReached = haveReached - 1;
                    }
                }
            }

            // check if we can stop moving
            if(haveReached == 3 && forward)
            {
                currentState = "reachedSeat";
            }
            else if(haveReached == 0 && !forward)
            {
                currentState = "beDestroyed";
            }
        }
    }

    public Color getColor()
    {
        return spriteRender.color;
    }

    void leaveTable()
    {
        // leaving should remove me from table
        if (angry)
        {
            mytable.GetComponent<Table>().removeAngryNPC(gameObject);
        }
        else
        {
            mytable.GetComponent<Table>().removeNPC(gameObject);
        }
    }

    public string getMyOrder()
    {
        if (myOrder == "")
        {
            createOrder();
        }

        return myOrder;
    }

    private void createOrder()
    {
        float ord = Random.value;
        if (ord < .5)
        {
            myOrder = "Booze";
        }
        else
        {
            myOrder = "CookedMeat";
        }
    }

    public void givenFood(string received)
    {
        if(currentState == "eating") { return; }
        currentState = "eating";

        // check if food is our correct order
        if (received != myOrder)
        {
            angry = true;
        }

        GameObject.FindObjectOfType<OrderTracker>().removeMyOrder(gameObject);
        timerStart = Time.time;
    }
}
