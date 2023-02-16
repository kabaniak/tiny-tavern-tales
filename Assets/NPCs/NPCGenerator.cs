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
    private float SpawnInterval = 25;
    private float SpawnTime = 26f;

    /// <summary>
    /// Check if we need to generate an NPC and if so, do so.
    /// </summary>
    void Update()
    {

        if (SpawnTime > SpawnInterval)
        {
            SpawnTime = 0;
            Instantiate(NPCPrefab, new Vector3(-31, 1, 0), Quaternion.identity);
        }
        SpawnTime += 0.05f;
        return;
    }
}
