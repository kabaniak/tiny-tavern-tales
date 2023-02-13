using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prepStation : MonoBehaviour
{
    public bool prepComplete;
    public bool holdingItem;
    public GameObject mask;
    public GameObject fill;
    private float fillSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        mask = GameObject.Find("Mask");
        fill = GameObject.Find("Fill");
        mask.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (holdingItem == true)
        //{
        //    gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        //    gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = true;
        //}
    }
}
