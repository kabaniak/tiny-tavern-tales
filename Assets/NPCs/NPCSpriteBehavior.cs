using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteBehavior : MonoBehaviour
{
    SpriteRenderer spriteRender;
    public float timeRemaining;
    public bool seated = false;

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

        // each time we update, subtract from time we'll wait
        timeRemaining = timeRemaining - Time.unscaledDeltaTime;
    }

    void leave()
    {

    }
}
