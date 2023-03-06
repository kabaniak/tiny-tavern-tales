using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Periodically creates a new NPC
/// </summary>
public class NPCGenerator : MonoBehaviour
{
    /// <summary>
    /// Object to spawn
    /// </summary>
    public GameObject NPCPrefab;

    private GameManager gm;

    private int[,] usedSprites = new int[3, 6] {
        { 0, 0, 0, 0, 0, 0 } ,
        { 0, 0, 0, 0, 0, 0 } ,
        { 0, 0, 0, 0, 0, 0 }
    };

    /// <summary>
    /// Seconds between spawn operations
    /// </summary>
    public float SpawnInterval;
    private float SpawnTime = 0;

    private float fastestInterval;
    private float slowestInterval;

    private List<int> npcQueue = new List<int>();
    public bool tutorial;
    public bool generating;

    int npcid = 0;

    private void Start()
    {
        fastestInterval = 5f;
        slowestInterval = 9f;
        SpawnInterval = 6f;
        generating = true;
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Check if we need to generate an NPC and if so, do so.
    /// </summary>
    void Update()
    {
        float change = (gm.reputation * 1.0f / gm.maxRating * 1.0f) * (fastestInterval - slowestInterval);
        SpawnInterval = slowestInterval + change;

        if (Time.time > SpawnTime & !tutorial & generating)
        {
            if (npcQueue.Count < 8 && isSpriteAvailable())
            {
                SpawnTime = Time.time + SpawnInterval;
                GameObject npc = Instantiate(NPCPrefab, new Vector3(-11.2f, -20, 0), Quaternion.identity);
                npc.GetComponent<NPCSpriteBehavior>().setId(npcid);
                npcQueue.Add(npcid);
                npcid += 1;
            }
        }
        return;
    }

    public void removeFromQueue(int npc)
    {
        npcQueue.Remove(npc);
    }

    public int getPlaceInQueue(int npc)
    {
        for(int i=0; i<npcQueue.Count; i++)
        {
            if(npcQueue[i] == npc) { return i; }
        }
        return -1;
    }

    public bool amFirst(int npc)
    {
        if (tutorial) { return true; }
        return (npcQueue[0] == npc) ;
    }

    public void resetQueue()
    {
        npcQueue = new List<int>();
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                usedSprites[j, i] = 0;
            }
        }
    }

    public void changeInterval(int day)
    {
        if(day == 2)
        {
            slowestInterval = 8f;
            fastestInterval = 4f;
        }
        if(day == 3)
        {
            slowestInterval = 6f;
            fastestInterval = 2.5f;
        }
    }

    public int getAvailableType()
    {
        int typenum = Random.Range(0, 3);
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                if (usedSprites[typenum, i] == 0) { return typenum; }
            }
            typenum++;
            if(typenum > 2) { typenum = 0; }
        }
    }

    private bool isSpriteAvailable()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                if (usedSprites[j, i] == 0) { return true; }
            }
        }
        return false;
    }
    public int getAvailableSprite(int type)
    {
        for (int i = 0; i < 6; i++)
        {
            if (usedSprites[type, i] == 0) { return i; }
        }
        return -1;
    }

    public void takeSprite(int type, int sprite)
    {
        usedSprites[type, sprite] = 1;
    }

    public void freeSprite(int type, int sprite)
    {
        usedSprites[type, sprite] = 0;
    }
}
