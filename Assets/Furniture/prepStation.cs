using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prepStation : MonoBehaviour
{
    public bool prepComplete;
    public bool holdingItem;
    public GameObject mask;
    public GameObject fill;

    // Start is called before the first frame update
    void Start()
    {
        mask = GameObject.Find("PrepMask");
        fill = GameObject.Find("PrepFill");
        mask.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingItem == true)
        {
            mask.SetActive(true);
        }
    }
}
