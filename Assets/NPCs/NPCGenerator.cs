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

    //Used Sprites
    public List<Sprite> UsedHumanSprites;
    public List<Sprite> UsedTieflingSprites;
    public List<Sprite> UsedElfSprites;

    /// <summary>
    /// Check if we need to generate an NPC and if so, do so.
    /// </summary>
    void Update()
    {

        if (Time.time > SpawnTime)
        {
            SpawnTime = Time.time + SpawnInterval;
            Instantiate(NPCPrefab, new Vector3(-10, -20, 0), Quaternion.identity);
        }
        return;
    }
}
