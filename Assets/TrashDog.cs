using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDog : MonoBehaviour
{

    public Sprite heart_sprite;
    public Sprite normalsprite;
    private int time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>().sprite == heart_sprite)
        {
            if (time >= 100)
            {
                GetComponent<SpriteRenderer>().sprite = normalsprite;
            }

            else
            {
                time++;
            }
        }
    }
}
