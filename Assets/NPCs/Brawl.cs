using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brawl : MonoBehaviour
{
    GameObject[] involvedNPCs = { null, null };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startBrawl(GameObject npc1, GameObject npc2)
    {
        involvedNPCs[0] = npc1;
        involvedNPCs[1] = npc2;

        foreach (GameObject o in involvedNPCs)
        {
            o.GetComponent<NPCSpriteBehavior>().enterBrawl();
        }
    }
}
