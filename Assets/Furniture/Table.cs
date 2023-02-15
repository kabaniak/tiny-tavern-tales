using UnityEngine;

/// <summary>
/// Periodically creates a new NPC
/// </summary>
public class Table : MonoBehaviour
{

    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    public GameObject PreppedMeatPrefab;
    public GameObject CoinPrefab;
    public GameObject CookedMeatPrefab;
    public GameObject BurntMeatPrefab;

    /// <summary>
    /// Array of the NPCs seated at the table
    /// </summary>
    public GameObject[] atTable = { null, null, null, null };

    /// Array of food on the table
    public GameObject[] foodServed = { null, null, null, null };
    public string[] foodTypes = { "", "", "", "" };
    private bool[] locked = { false, false, false, false };

    /// <summary>
    /// nothing for now
    /// </summary>
    void Update()
    {
        for (int i = 0; i< 4; i++)
        {
            GameObject temp = foodServed[i];
            if (foodServed[i] != null && atTable[i] != null && foodServed[i].GetComponent<Coin>() == null)
            {
                //if(foodServed[i].tag == "Meat" || foodServed[i].tag == "")
                atTable[i].GetComponent<NPCSpriteBehavior>().hasFood = true;
                locked[i] = true;
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
            if(atTable[i] == null && foodServed[i] == null) { return true; }
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
                if (atTable[i] == null && foodServed[i] == null) 
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
                int price;
                int rep;

                if (foodTypes[i] == atTable[i].GetComponent<NPCSpriteBehavior>().getMyOrder())
                {
                    // order is correct
                    price = (int)GameManager.prices.correct;
                    rep = (int)GameManager.popularity.correct;
                }
                else
                {
                    // order is incorrect
                    price = (int)GameManager.prices.incorrect;
                    rep = (int)GameManager.popularity.incorrect;
                }
                
                atTable[i] = null;
                foodTypes[i] = "";
                Destroy(foodServed[i]);

                foodServed[i] = createOnTable(i, CoinPrefab);
                foodServed[i].transform.localScale = new Vector3(0.1f, 0.1f, 1);
                foodServed[i].GetComponent<Coin>().myTable = gameObject;

                foodServed[i].GetComponent<Coin>().value[0] = price;
                foodServed[i].GetComponent<Coin>().value[1] = rep;


                return;
            }
        }
    }

    public void removeAngryNPC(GameObject npc)
    {
        for (int i = 0; i < 4; i++)
        {
            if (atTable[i] == npc)
            {
                atTable[i] = null;
                locked[i] = false;
                return;
            }
        }
    }

    public string getFoodServedToNPC(GameObject npc)
    {
        for (int i = 0; i < 4; i++)
        {
            if (atTable[i] == npc)
            {
                return foodTypes[i];
            }
        }
        return "";
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

    public Vector3 getSeatCoordsByInd(int i)
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

    public int getClosestInd(GameObject o)
    {
        int closest = 0;
        float mindist = 4000;

        // figure out seat we're closest too
        for (int i = 0; i < 4; i++)
        {
            Vector3 seatPos = getSeatCoordsByInd(i);
            float dist = Vector3.Distance(seatPos, o.transform.position);

            if (dist <= mindist)
            {
                closest = i;
                mindist = dist;
            }

        }
        return closest;
    }

    public void serveSeat(GameObject o)
    {
        int closest = getClosestInd(o);

        if (o.tag == "Player1")
        {
            player1control player = o.GetComponent<player1control>();

            GameObject toCreate = BoozePrefab;
            if (player.currentObject == ""){
                if (foodServed[closest] != null && !locked[closest])
                {
                    if (foodTypes[closest] == "Meat")
                    {
                        player.pickupFromSource(o, MeatPrefab);
                        player.currentObject = foodTypes[closest];
                    }
                    else if (foodTypes[closest] == "PreppedMeat")
                    {
                        player.pickupFromSource(o, PreppedMeatPrefab);
                        player.currentObject = foodTypes[closest];
                    }
                    else if (foodTypes[closest] == "CookedMeat")
                    {
                        player.pickupFromSource(o, CookedMeatPrefab);
                        player.currentObject = foodTypes[closest];
                    }
                    else if (foodTypes[closest] == "BurntMeat")
                    {
                        player.pickupFromSource(o, BurntMeatPrefab);
                        player.currentObject = foodTypes[closest];
                    }
                    else
                    {
                        player.pickupFromSource(o, BoozePrefab);
                        player.currentObject = foodTypes[closest];
                    }

                    Destroy(foodServed[closest]);
                    foodServed[closest] = null;
                    foodTypes[closest] = "";
                    player.canServe = true;
                    player.servable = this;
                    return;
                }
            }
            else if (player.currentObject == "Meat")
            {
                toCreate = MeatPrefab;
            }
            else if (player.currentObject == "PreppedMeat")
            {
                toCreate = PreppedMeatPrefab;
            }
            else if (player.currentObject == "CookedMeat")
            {
                toCreate = CookedMeatPrefab;
            }
            else if (player.currentObject == "BurntMeat")
            {
                toCreate = BurntMeatPrefab;
            }

            if (foodServed[closest] == null)
            {
                foodServed[closest] = createOnTable(closest, toCreate);
                foodTypes[closest] = player.currentObject;
                player.FeedtheDog();
            }
        }
        else if(o.tag == "Player2")
        {
            player2control player = o.GetComponent<player2control>();

            GameObject toCreate = BoozePrefab;
            if (player.currentObject == "")
            {
                if (foodServed[closest] != null && !locked[closest])
                {
                    if (foodTypes[closest] == "Meat")
                    {
                        player.pickupFromSource(o, MeatPrefab);
                        player.currentObject = foodTypes[closest];
                    }
                    else if (foodTypes[closest] == "PreppedMeat")
                    {
                        player.pickupFromSource(o, PreppedMeatPrefab);
                        player.currentObject = foodTypes[closest];
                    }
                    else if (foodTypes[closest] == "CookedMeat")
                    {
                        player.pickupFromSource(o, CookedMeatPrefab);
                        player.currentObject = foodTypes[closest];
                    }
                    else if (foodTypes[closest] == "BurntMeat")
                    {
                        player.pickupFromSource(o, BurntMeatPrefab);
                        player.currentObject = foodTypes[closest];
                    }
                    else
                    {
                        player.pickupFromSource(o, BoozePrefab);
                        player.currentObject = foodTypes[closest];
                    }

                    Destroy(foodServed[closest]);
                    foodServed[closest] = null;
                    player.canServe = true;
                    player.servable = this;
                    foodTypes[closest] = "";
                    return;
                }
            }
            else if (player.currentObject == "Meat")
            {
                toCreate = MeatPrefab;
            }
            else if (player.currentObject == "PreppedMeat")
            {
                toCreate = PreppedMeatPrefab;
            }
            else if (player.currentObject == "CookedMeat")
            {
                toCreate = CookedMeatPrefab;
            }
            else if (player.currentObject == "BurntMeat")
            {
                toCreate = BurntMeatPrefab;
            }

            if (foodServed[closest] == null)
            {
                foodServed[closest] = createOnTable(closest, toCreate);
                foodTypes[closest] = player.currentObject;
                player.FeedtheDog();
            }
        }
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

    public void removeCoin(GameObject coin)
    {
        for (int i=0; i< 4; i++)
        {
            if( foodServed[i] == coin)
            {
                Destroy(foodServed[i]);
                foodServed[i] = null;
                locked[i] = false;
                return;
            }
        }
    }
}
