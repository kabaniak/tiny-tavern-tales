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
}
