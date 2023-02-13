using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prepStation : MonoBehaviour
{
    public bool prepComplete;
    public bool holdingItem;
    public GameObject mask;
    public GameObject fill;
    public GameObject p1;
    public GameObject p2;

    // Start is called before the first frame update
    void Start()
    {
        mask = GameObject.Find("PrepMask");
        fill = GameObject.Find("PrepFill");
        p1 = GameObject.Find("p1");
        p2 = GameObject.Find("p2");
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingItem == true)
        {
            mask.SetActive(true);
            p1.transform.GetComponent<player1control>().pfill = fill;
        }
    }
}
