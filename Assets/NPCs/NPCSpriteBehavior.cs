using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteBehavior : MonoBehaviour
{
    SpriteRenderer spriteRender;
    public float timeRemaining;
    public int speed = 5;

    public bool seated = false;
    public bool reachedSeat = false;
    public bool hasFood = false;
    public bool leaving = false;

    public Vector3 seatCoords;
    public GameObject mytable;

    public bool startedTimer = false;
    public float tempTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        // the amount of time this npc will wait for
        timeRemaining = 40;
    }

    // Update is called once per frame
    void Update()
    {
        // check if we're on our way out
        if (leaving)
        {
            // if we're at door level just head out
            if (transform.position.y < 1 && transform.position.y > -1)
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

        // check if we've been served our food
        if (hasFood)
        {
            // start an eat "timer" before it leaves
            if ( ! startedTimer)
            {
                startedTimer = true;
            }

            // if timer has run out it should leave
            if (tempTimer < 0)
            {
                leaveTable();
            }

            tempTimer -= Time.unscaledDeltaTime;
        }

        // if we won't wait anymore
        if (timeRemaining <= 0)
        {
            // should leave (will do later)
            
        }

        // if not seated yet
        if (!seated)
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
        if (seated && !reachedSeat)
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

        // each time we update, subtract from time we'll wait
        timeRemaining = timeRemaining - Time.unscaledDeltaTime;
    }

    void leaveTable()
    {
        // leaving should remove me from table
        mytable.GetComponent<Table>().removeNPC(gameObject);

        // indicate that we're leaving now
        leaving = true;
    }
}
