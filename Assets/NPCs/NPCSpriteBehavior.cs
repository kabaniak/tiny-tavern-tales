using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteBehavior : MonoBehaviour
{
    SpriteRenderer spriteRender;
    Color orig;
    public float timeRemaining;
    public int speed = 5;

    // booleans describing the npc's state
    public bool seated = false;
    public bool reachedSeat = false;
    public bool hasFood = false;
    public bool leaving = false;
    public bool orderAdded = false;
    public bool angry = false;

    // information about where the npc is seated
    public Vector3 seatCoords;
    public GameObject mytable;

    // information about the npcs order
    private string myOrder = "";

    // temporary timer info
    public bool startedTimer = false;
    public float tempTimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        // the amount of time this npc will wait for
        timeRemaining = 40;
        spriteRender = GetComponent<SpriteRenderer>();

        orig = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 0.7f);
        spriteRender.color = orig;

    }

    // Update is called once per frame
    void Update()
    {
        // check if we're on our way out
        if (leaving)
        {
            if(transform.position.x < -31.1)
            {
                Destroy(gameObject);
            }
            // if we're at door level just head out
            else if (transform.position.y < 2 && transform.position.y > 1)
            {
                transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
            }
            // in these two seats you move horizontal first
            else if (seatCoords.y > 11 || seatCoords.y < -10)
            {
                if (transform.position.x > -25.7)
                {
                    transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
                }
                // need to get to correct x position
                else
                {
                    if (seatCoords.y > 0)
                    {
                        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;
                    }
                }
            }
            // otherwise horizontal first
            else
            {
                if (seatCoords.y > 0)
                {
                    transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
                }
                else
                {
                    transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;
                }
            }
        }

        // if it's reached it's seat
        // and if we've been served our food
        else if (reachedSeat && hasFood)
        {
            // start an eat "timer" before it leaves
            if ( ! startedTimer)
            {
                // check if food is our correct order
                string received = mytable.GetComponent<Table>().getFoodServedToNPC(gameObject);
                if (received != myOrder)
                {
                    spriteRender.color = new Color(1, 0, 0, 1.75f);
                    angry = true;
                }

                startedTimer = true;
            }

            if (angry)
            {
                spriteRender.color = Color.Lerp(new Color(1, 0, 0, 1.75f), orig, tempTimer / 5.0f);
            }

            // if timer has run out it should leave
            if (tempTimer < 0 && !leaving)
            {
                leaveTable();
                leaving = true;
            }

            tempTimer -= Time.fixedDeltaTime;
        }

        // if we won't wait anymore
        else if (timeRemaining <= 0)
        {
            // should leave (will do later)
            
        }

        // if not seated yet
        else if (!seated)
        {
            // try to find a table to go to
            GameObject[] tables = GameObject.FindGameObjectsWithTag("table");

            foreach(GameObject t in tables)
            {
                if (t.GetComponent<Table>().seatNPC(gameObject))
                {
                    mytable = t;
                    seated = true;
                    seatCoords = t.GetComponent<Table>().getSeatCoords(gameObject);
                    break;
                }
            }

            // if no table to go to
            // pace up and down
        }

        // if not at table yet
        else if (seated && !reachedSeat)
        {
            // head to seat coordinates
            if (transform.position.x < -25.7) {
                transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
            }
            // in these two seats you move vertical first
            else if (seatCoords.y > 11 || seatCoords.y < -10)
            {
                if (transform.position.y <= seatCoords.y + .1 && transform.position.y >= seatCoords.y - .1)
                {
                    if (transform.position.x >= seatCoords.x)
                    {
                        reachedSeat = true;
                    }
                    else
                    {
                        transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
                    }
                }
                // need to get to correct x position
                else
                {
                    if (seatCoords.y > 0)
                    {
                        transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
                    }
                }
            }
            // otherwise horizontal first
            else
            {
                if (transform.position.x >= seatCoords.x)
                {
                    if (transform.position.y <= seatCoords.y + .1 && transform.position.y >= seatCoords.y - .1)
                    {
                        reachedSeat = true;
                    }
                    else
                    {
                        if (seatCoords.y > 0)
                        {
                            transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;
                        }
                        else
                        {
                            transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
                        }
                    }
                }
                // need to get to correct x position
                else
                {
                    transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
                }
            }


        }

        else if (reachedSeat && !orderAdded)
        {
            OrderTracker ot = GameObject.FindObjectOfType<OrderTracker>();
            ot.addOrder(gameObject);
            orderAdded = true;
        }

        // each time we update, subtract from time we'll wait
        //timeRemaining = timeRemaining - Time.unscaledDeltaTime;
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
}
