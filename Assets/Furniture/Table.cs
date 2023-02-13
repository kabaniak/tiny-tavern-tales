using UnityEngine;

/// <summary>
/// Periodically creates a new NPC
/// </summary>
public class Table : MonoBehaviour
{

    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    public GameObject CoinPrefab;

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
        for (int i = 0; i< 4; i++)
        {
            if (foodServed[i] != null && atTable[i] != null)
            {
                atTable[i].GetComponent<NPCSpriteBehavior>().hasFood = true;
            }
        }
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
                Destroy(foodServed[i]);
                foodServed[i] = null;
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
        if(collision.gameObject.tag == "Player1")
        {
            // activate ability to serve a seat
            player1control player = collision.gameObject.GetComponent<player1control>();

            player.canServe = true;
            player.servable = gameObject.GetComponent<Table>();
        }
        else if (collision.gameObject.tag == "Player2")
        {
            // activate ability to serve a seat
            player2control player = collision.gameObject.GetComponent<player2control>();

            player.canServe = true;
            player.servable = gameObject.GetComponent<Table>();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1")
        {
            // deactivate ability to serve a seat
            player1control player = collision.gameObject.GetComponent<player1control>();

            player.canServe = false;
            player.servable = null;
        }
        else if (collision.gameObject.tag == "Player2")
        {
            // deactivate ability to serve a seat
            player2control player = collision.gameObject.GetComponent<player2control>();

            player.canServe = false;
            player.servable = null;
        }
    }

    public bool serveSeat(GameObject o)
    {
        if (o.tag == "Player1")
        {
            player1control player = o.GetComponent<player1control>();

            GameObject toCreate = BoozePrefab;
            if (player.currentObject == ""){
                return false;
            }
            else if (player.currentObject == "Meat")
            {
                toCreate = MeatPrefab;
            }

            // seat 0
            if (player.transform.position.x < transform.position.x - 1.3)
            {
                foodServed[0] = createOnTable(0, toCreate);
                player.FeedtheDog();
            }
            // seat 1
            else if (player.transform.position.y < transform.position.y - 1.3)
            {
                foodServed[1] = createOnTable(1, toCreate);
                foodServed[1].GetComponent<Renderer>().sortingOrder = 0;
                player.FeedtheDog();
            }
            // seat 2
            else if (player.transform.position.x > transform.position.x + 1.3)
            {
                foodServed[2] = createOnTable(2, toCreate);
                player.FeedtheDog();
            }
            // seat 3
            else if (player.transform.position.y > transform.position.y + 1.3)
            {
                foodServed[3] = createOnTable(3, toCreate);
                player.FeedtheDog();
            }
        }
        else if(o.tag == "Player2")
        {
            player2control player = o.GetComponent<player2control>();

            GameObject toCreate = BoozePrefab;
            if (player.currentObject == "")
            {
                return false;
            }
            else if (player.currentObject == "Meat")
            {
                toCreate = MeatPrefab;
            }

            // seat 0
            if (player.transform.position.x < transform.position.x - 1.3)
            {
                foodServed[0] = createOnTable(0, toCreate);
                player.FeedtheDog();
            }
            // seat 1
            else if (player.transform.position.y < transform.position.y - 1.3)
            {
                foodServed[1] = createOnTable(1, toCreate);
                foodServed[1].GetComponent<Renderer>().sortingOrder = 0;
                player.FeedtheDog();
            }
            // seat 2
            else if (player.transform.position.x > transform.position.x + 1.3)
            {
                foodServed[2] = createOnTable(2, toCreate);
                player.FeedtheDog();
            }
            // seat 3
            else if (player.transform.position.y > transform.position.y + 1.3)
            {
                foodServed[3] = createOnTable(3, toCreate);
                player.FeedtheDog();
            }
        }

        return false;
    }

    public GameObject createOnTable(int seat, GameObject source)
    {
        float xCoord = transform.position.x;
        if(seat %2 == 0)
        {
            xCoord += 2f * (seat - 1);
        }

        float yCoord = transform.position.y + 0.5f;
        if (seat % 2 != 0)
        {
            yCoord += 1.5f * (seat - 2);
        }
        

        GameObject thing = Instantiate(source, new Vector3(xCoord, yCoord, 0), Quaternion.identity, transform);
        thing.transform.localScale = new Vector3(3, 3, 1);
        return thing;
    }
}
