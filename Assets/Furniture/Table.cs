using UnityEngine;

/// <summary>
/// Periodically creates a new NPC
/// </summary>
public class Table : MonoBehaviour
{
    /// <summary>
    /// Array of the NPCs seated at the table
    /// </summary>
    public GameObject[] atTable = { null, null, null, null };

    /// Array of food on the table
    public GameObject[] foodServed = { null, null, null, null };

    /// <summary>
    /// nothing for now
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// Check if we have space at the table
    /// </summary>
    bool hasSpace()
    {
        for (int i=0; i< 4; i++)
        {
            if(atTable[i] == null) { return true; }
        }
        return false;
    }

    /// <summary>
    /// Check if we have space at the table
    /// </summary>
    public bool seatNPC(GameObject npc)
    {
        if (hasSpace())
        {
            for (int i = 0; i < 4; i++)
            {
                if (atTable[i] == null) 
                {
                    atTable[i] = npc;
                    return true;
                }
            }
        }
        return false;
    }

    public void removeNPC(GameObject npc)
    {
        for (int i = 0; i < 4; i++)
        {
            if (atTable[i] == npc)
            {
                atTable[i] = null;
                foodServed = null;
                return;
            }
        }
    }

    public Vector3 getSeatCoords(GameObject npc)
    {
        for (int i = 0; i < 4; i++)
        {
            if (atTable[i] == npc)
            {
                float xCoord = gameObject.transform.position.x;
                if (i % 2 == 0)
                {
                    xCoord += (i - 1) * 5;
                }
                float yCoord = gameObject.transform.position.y;
                yCoord += 2;
                if (i % 2 != 0)
                {
                    yCoord += (i - 2) * 5;
                    if (i == 3)
                    {
                        yCoord -= 2;
                    }
                }

                return new Vector3(xCoord, yCoord, 0);
            }
        }
        return new Vector3(-31, 1, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            // activate ability to serve a seat
            GameObject player = collision.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            // deactivate ability to serve a seat
            GameObject player = collision.gameObject;
        }
    }

    public bool serveSeat(GameObject o)
    {
        PlayerControl player = o.GetComponent<PlayerControl>();
        // seat 0
        if (player.transform.position.x < transform.position.x - 1.3)
        {
            //foodServed[0] = player.currentObject;
        }
        // seat 1
        else if (player.transform.position.y < transform.position.y - 1.3)
        {
            //foodServed[1] = player.currentObject;
        }
        // seat 2
        else if (player.transform.position.x > transform.position.x + 1.3)
        {
            //foodServed[2] = player.currentObject;
        }
        // seat 3
        else if (player.transform.position.y > transform.position.y + 1.3)
        {
            //foodServed[3] = player.currentObject;
        }
        return false;
    }
}
