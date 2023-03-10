using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class prepStation : MonoBehaviour
{
    public bool prepComplete;
    public bool holdingItem;
    public GameObject mask;
    public GameObject fill;
    public GameObject p1;
    public GameObject p2;
    public GameObject PreppedMeatPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mask = GameObject.Find("PrepMask");
        fill = GameObject.Find("PrepFill");
        p1 = GameObject.Find("p1");
        p2 = GameObject.Find("p2");
        holdingItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (prepComplete == true)
        {
            holdingItem = false;
            prepComplete = false;
            Destroy(gameObject.transform.GetChild(0).gameObject);
            fill.transform.GetComponent<Image>().fillAmount = 0;
        }
    }

    public void resetPrep()
    {
        holdingItem = false;
        prepComplete = false;
        if (gameObject.transform.childCount > 0) { Destroy(gameObject.transform.GetChild(0).gameObject); }
        fill.transform.GetComponent<Image>().fillAmount = 0;
        mask.transform.GetComponent<Image>().color = new Color(1,1,1,0);
    }
}
