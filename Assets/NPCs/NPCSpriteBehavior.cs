using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteBehavior : MonoBehaviour
{
    SpriteRenderer spriteRender;
    public float timeRemaining;
    public bool seated = false;
    public bool reachedSeat = false;
    public int speed = 5;

    public Vector3 seatCoords;

    public GameObject mytable;

    // Start is called before the first frame update
    void Start()
    {
        // the amount of time this npc will wait for
        timeRemaining = 30;
    }

    // Update is called once per frame
    void Update()
    {
        // if won't wait anymore
        if (timeRemaining <= 0)
        {
            leave();
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

    void leave()
    {
        // leaving doesn't actually leave yet
    }
}
