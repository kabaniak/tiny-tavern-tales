using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteBehavior : MonoBehaviour
{
    SpriteRenderer spriteRender;
    public GameObject gameManager;
    Color angryColor = new Color(1, 0, 0, 1.75f);
    Color orig;

    // information about the npc's stats
    public int id;
    private string myOrder = ""; // their order
    private float speed = 0.1f; // how fast they move
    private bool angry = false;
    private float origPatience;
    public float patience; // how willing they are to wait
    private float tolerance; // how quickly they lose patience
    private float hardOrder;
    private float brawlChance;

    // string representing the npc's state
    // either: waiting, toSeat, reachedSeat, ordered, eating, leaving
    public string currentState = "waiting";
    private int currPos = -1;

    // information about where the npc is seated
    public Vector3 seatCoords;
    public GameObject mytable;

    private string NPCtype;
    private int haveReached = 0;
    private float sensitivity = 0.1f;
    private bool forward = true;
    private Vector2[] path = { new Vector2(-7.5f, -20), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

    // brawl information
    public GameObject brawlPartner = null;
    public GameObject BrawlPrefab;

    // temporary timer info
    public float timerStart = 0;

    //NPC Sprites
    public List<Sprite> HumanSprites;
    public List<Sprite> TieflingSprites;
    public List<Sprite> ElfSprites;
    int spritenum;
    int rolenum;

    // tutorial stuff
    private bool pausedPt = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GManager");

        //choose NPC type
        // pick a type from screen with an unused sprite
        setNPCType(GameObject.FindObjectOfType<NPCGenerator>().getAvailableType());
    }

    public void setId(int num)
    {
        id = num;
    }

    void setNPCType(int typenum)
    {
        rolenum = typenum;
        if (typenum == 0)
        {
            NPCtype = "HumanFighter";
            tolerance = 1.0f;
            patience = 40f;
            brawlChance = 0.7f;
            hardOrder = 0.2f;
        }
        else if (typenum == 1)
        {
            NPCtype = "TieflingSorcerer";
            tolerance = 1.0f;
            patience = 70f;
            brawlChance = 0.2f;
            hardOrder = 0.5f;
        }
        else if (typenum == 2)
        {
            NPCtype = "ElfRogue";
            tolerance = 1.0f;
            patience = 50f;
            brawlChance = 0.4f;
            hardOrder = 0.35f;
        }
        origPatience = patience;


        //Choose NPC sprite
        spritenum = GameObject.FindObjectOfType<NPCGenerator>().getAvailableSprite(typenum);
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

        GameObject.FindObjectOfType<NPCGenerator>().takeSprite(typenum, spritenum);
    }

    // Update is called once per frame
    void Update()
    {
        // check if we're ready to be destroyed
        if (currentState == "beDestroyed")
        {
            GameObject.FindObjectOfType<NPCGenerator>().freeSprite(rolenum, spritenum);
            Destroy(gameObject);
        }

        if (currentState == "brawling" || currentState == "preBrawl") { return; }
        // if it's reached it's seat
        // and if we've been served our food
        else if (currentState == "eating")
        {
            if (angry)
            {
                spriteRender.color = Color.Lerp(orig, angryColor, (Time.time - timerStart) / 2f);
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

            // reset leave path
            // record the path we should take
            int seatInd = mytable.GetComponent<Table>().getSeatInd(gameObject);
            path[4] = new Vector2(seatCoords.x, seatCoords.y);
            path[3] = new Vector2(seatCoords.x, seatCoords.y);

            if (seatInd == 2)
            {
                path[1] = new Vector2(-7.5f, 0);
                path[2] = new Vector2(seatCoords.x, 0);
            }
            else
            {
                path[1] = new Vector2(-7.5f, seatCoords.y);
                path[2] = new Vector2(seatCoords.x, seatCoords.y);
            }
        }

        else if (currentState == "ordered")
        {
            if (patience <= 0)
            {
                angry = true;
                leaveTable();
                angerCounter();
                // random chance of starting a brawl
                if (Random.value < brawlChance)
                {
                    findBrawlTarget();
                }
                else
                {
                    currentState = "leaving";
                }
            }
        }

        // if not seated yet
        else if (currentState == "waiting" && currPos < 2)
        {
            NPCGenerator gen = GameObject.FindObjectOfType<NPCGenerator>();
            if (gen.amFirst(id))
            {
                // try to find a table to go to
                GameObject[] tables = GameObject.FindGameObjectsWithTag("table");

                foreach (GameObject t in tables)
                {
                    if (t.GetComponent<Table>().seatNPC(gameObject))
                    {
                        mytable = t;
                        currentState = "toSeat";
                        seatCoords = t.GetComponent<Table>().getSeatCoords(gameObject);
                        int seatInd = t.GetComponent<Table>().getSeatInd(gameObject);

                        GameObject.FindObjectOfType<NPCGenerator>().removeFromQueue(id);

                        // record the path we should take
                        path[4] = new Vector2(seatCoords.x, seatCoords.y);
                        path[3] = new Vector2(seatCoords.x, seatCoords.y);

                        if (transform.position.y < -12.5)
                        {
                            if (seatInd == 2)
                            {
                                path[1] = new Vector2(-11.2f, 0);
                                path[2] = new Vector2(seatCoords.x, 0);
                            }
                            else
                            {
                                path[1] = new Vector2(-11.2f, seatCoords.y);
                                path[2] = new Vector2(seatCoords.x, seatCoords.y);
                            }
                        }
                        else
                        {
                            path[1] = new Vector2(-7.5f, transform.position.y);
                            if (seatInd == 2)
                            {
                                path[2] = new Vector2(-7.5f, 0);
                                path[3] = new Vector2(seatCoords.x, 0);
                            }
                            else
                            {
                                path[2] = new Vector2(-7.5f, seatCoords.y);
                                path[3] = new Vector2(seatCoords.x, seatCoords.y);
                            }
                        }

                        break;
                    }

                }
            }

            // check if we won't wait anymore
            if (patience <= 0)
            {
                haveReached = 1;
                angry = true;
                currentState = "leaving";
                angerCounter();
                GameObject.FindObjectOfType<NPCGenerator>().removeFromQueue(id);
            }
        }

        // tutorial conditional
        if (!pausedPt)
        {
            patience -= tolerance * Time.deltaTime;
        }

        if (!angry && (currentState != "eating" && currentState != "leaving"))
        {
            spriteRender.color = Color.Lerp(angryColor, new Color(1, 1, 1), patience / (origPatience - 10f));
        }

        // check if restaurant is messy
        int count = GameObject.FindGameObjectsWithTag("Mess").Length;
        if(count == 0){ tolerance = 1.0f; }
        else if(count < 3){tolerance = 1.1f;}
        else if(count < 7) { tolerance = 1.25f; }
        else { tolerance = 1.5f; }

    }

    private void FixedUpdate()
    {
        // check if we're on our way out
        if (currentState == "preBrawl")
        {
            approach();
        }
        else if (currentState == "leaving")
        {
            forward = false;
            move();
        }
        // if not at table yet
        else if (currentState == "toSeat")
        {
            patience = origPatience;
            forward = true;
            move();
        }
        else if (currentState == "waiting")
        {
            // update wait pos
            updatePlaceInQueue();
        }
    }

    public void pausePatience(bool on)
    {
        pausedPt = on;
    }

    public void setPatience(float pt)
    {
        patience = pt;
        origPatience = pt + 10f;
    }

    public void setBrawlChance(float chance)
    {
        brawlChance = chance;
    }


    void move()
    {
        if (currentState == "leaving" || currentState == "toSeat")
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
            if (haveReached == 4 && forward)
            {
                currentState = "reachedSeat";
            }
            else if (haveReached == 0 && !forward)
            {
                currentState = "beDestroyed";
            }
        }
    }

    void approach()
    {
        // check if we've reached the npc
        if (transform.position.x - sensitivity < brawlPartner.transform.position.x && transform.position.x + sensitivity > brawlPartner.transform.position.x)
        {
            if (transform.position.y - sensitivity < brawlPartner.transform.position.y && transform.position.y + sensitivity > brawlPartner.transform.position.y)
            {
                startABrawl();
                return;
            }
        }

        Vector3 currPos = transform.position;
        Vector2 dir = new Vector2(brawlPartner.transform.position.x - currPos.x, brawlPartner.transform.position.y - currPos.y);
        dir.Normalize();

        transform.position = new Vector3(currPos.x + dir.x * 0.05f, currPos.y + dir.y * 0.05f, 0);
    }

    void updatePlaceInQueue()
    {
        int place = GameObject.FindObjectOfType<NPCGenerator>().getPlaceInQueue(id);
        if(place != -1 && place < 8 && currPos != place)
        {
            float desY = 11.3f - place * 3;
            if(transform.position.y > desY - sensitivity && transform.position.y < desY + sensitivity)
            {
                currPos = place;
            }
            else
            {
                Vector3 currPos = transform.position;
                Vector2 dir = new Vector2(0, desY - currPos.y);
                dir.Normalize();

                transform.position = new Vector3(currPos.x + dir.x * speed, currPos.y + dir.y * speed, 0);
            }
        }
    }

    public int getSpriteNum()
    {
        return spritenum;
    }

    public string getSpriteType()
    {
        return NPCtype;
    }

    public string getCurrentState() { return currentState; }

    public bool isAngry() { return angry; }

    void leaveTable()
    {

        GameObject.FindObjectOfType<OrderTracker>().removeMyOrder(gameObject);

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
        if (ord > hardOrder)
        {
            myOrder = "Booze";
        }
        else
        {
            myOrder = "CookedMeat";
        }
    }

    public void setOrder(bool beer)
    {
        if (beer) { myOrder = "Booze"; }
        else { myOrder = "CookedMeat"; }
    }

    public void givenFood(string received)
    {
        if (currentState == "eating") {return;}
        if (currentState == "brawling" || currentState == "preBrawl")
        {
            if (mytable != null)
            {
                angry = true;
                leaveTable();
                angerCounter();
            }
            mytable = null;
            return;
        }


        currentState = "eating";

        // check if food is our correct order
        if (received != myOrder)
        {
            angry = true;
            angerCounter();
            if (Random.value < brawlChance)
            {
                findBrawlTarget();
            }
        }
        else
        {
            spriteRender.color = new Color(1, 1, 1);
            gameManager.GetComponent<GameManager>().servedToday += 1;
        }

        GameObject.FindObjectOfType<OrderTracker>().removeMyOrder(gameObject);
        timerStart = Time.time;
        orig = spriteRender.color;

    }

    private void startABrawl()
    {
        if (currentState == "preBrawl")
        {
            // this is called when we have reached the target
            // instantiate a brawl sprite
            float xpos = transform.position.x + (brawlPartner.transform.position.x - transform.position.x) / 2;
            float ypos = transform.position.y + (brawlPartner.transform.position.y - transform.position.y) / 2;
            GameObject br = Instantiate(BrawlPrefab, new Vector3(xpos, ypos, 0), Quaternion.identity);

            // tell the brawl that it's started
            br.GetComponent<Brawl>().startBrawl(gameObject, brawlPartner);
            gameManager.GetComponent<GameManager>().brawlsToday += 1;
        }
    }

    private void findBrawlTarget ()
    {
        NPCSpriteBehavior closest = null;
        float minDist = float.PositiveInfinity;

        // find nearest npc
        foreach (NPCSpriteBehavior o in GameObject.FindObjectsOfType<NPCSpriteBehavior>())
        {
            // if not us
            if (o != gameObject.GetComponent<NPCSpriteBehavior>())
            {
                Vector2 dist = new Vector2(o.transform.position.x - transform.position.x, o.transform.position.y - transform.position.y);
                float distval = Mathf.Sqrt(dist.x * dist.x + dist.y * dist.y);
                if (minDist > distval)
                {
                    minDist = distval;
                    closest = o;
                }

            }
        }

        if (closest != null)
        {
            if (closest.engageInBrawl(gameObject))
            {
                brawlPartner = closest.gameObject;
                currentState = "preBrawl";
                spriteRender.color = angryColor;
            }
        }

    }

    public bool engageInBrawl(GameObject npc)
    {
        // if not already in a brawl and on the screen
        if (brawlPartner == null && transform.position.y > -15.5)
        {
            brawlPartner = npc;
            currentState = "preBrawl";
            angry = true;
            angerCounter();
            if (mytable)
            {
                leaveTable();
                mytable = null;
            }
            spriteRender.color = angryColor;
            return true;
        }
        return false;
    }
    //
    public void angerCounter()
    {
        gameManager.GetComponent<GameManager>().angryToday += 1;
    }
    public void enterBrawl()
    {
        // stop rendering npc
        spriteRender.enabled = false;

        currentState = "brawling";
    }
    public void exitBrawl()
    {
        // stop rendering npc
        spriteRender.enabled = true;

        // set our new leave path
        Vector3 mypos = transform.position;
        
        // if we're spat out to the right of the table center
        if(2.4 < mypos.x)
        {
            // if our y means we can't just go left
            if((-2.4> mypos.y && -8< mypos.y) || (14 > mypos.y && 9 < mypos.y))
            {
                // go down or up first
                if ((mypos.y > -5.2 && mypos.y < 0) || (mypos.y < 11.5 && mypos.y > 0))
                {
                    path[2] = new Vector2(mypos.x, 0);
                    path[1] = new Vector2(-7.5f, 0);
                }
                else
                {
                    if(mypos.y > 0)
                    {
                        path[2] = new Vector2(mypos.x, 15);
                        path[1] = new Vector2(-7.5f, 15);
                    }
                    else
                    {
                        path[2] = new Vector2(mypos.x, -10);
                        path[1] = new Vector2(-7.5f, -10);
                    }
                }
            }
            else
            {
                path[1] = new Vector2(-7.5f, mypos.y);
                path[2] = new Vector2(-7.5f, mypos.y);
            }

        }
        else
        {
            path[1] = new Vector2(-7.5f, mypos.y);
            path[2] = new Vector2(-7.5f, mypos.y);
        }
        haveReached = 3;
        currentState = "leaving";
    }
}
