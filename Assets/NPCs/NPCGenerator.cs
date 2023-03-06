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

    /// <summary>
    /// Seconds between spawn operations
    /// </summary>
    private float SpawnInterval = 8f;
    private float SpawnTime = 0;

    private List<int> npcQueue = new List<int>();

    int npcid = 0;

    /// <summary>
    /// Check if we need to generate an NPC and if so, do so.
    /// </summary>
    void Update()
    {

        if (Time.time > SpawnTime)
        {
            SpawnTime = Time.time + SpawnInterval;
            GameObject npc = Instantiate(NPCPrefab, new Vector3(-11.2f, -20, 0), Quaternion.identity);
            npc.GetComponent<NPCSpriteBehavior>().setId(npcid);
            npcQueue.Add(npcid);
            npcid += 1;
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
        return (npcQueue[0] == npc) ;
    }
}
