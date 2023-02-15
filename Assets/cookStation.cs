using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cookStation : MonoBehaviour
{
    private string cookState;
    private float cookSpeed = 0.15f;
    private float overcooked;
    private float burnTime;
    public bool burnt;
    public bool cooked;
    public bool holdingItem;
    public bool doomsday;
    public bool stop;
    public GameObject mask;
    public GameObject fill;
    public GameObject p1;
    public GameObject p2;
    public GameObject PreppedMeatPrefab;
    public GameObject CookedMeatPrefab;
    public GameObject BurntMeatPrefab;
    private Color good;
    private Color bad;

    // Start is called before the first frame update
    void Start()
    {
        mask = GameObject.Find("CookMask");
        fill = GameObject.Find("CookFill");
        p1 = GameObject.Find("p1");
        p2 = GameObject.Find("p2");
        holdingItem = false;
        good = Color.green;
        bad = Color.red;
        burnTime = 5f;
        burnt = false;
        doomsday = false;
        stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingItem == true)
        {
            mask.SetActive(true);
        }

        if (holdingItem == true & cooked == false & burnt == false)
        {
            p1.transform.GetComponent<player1control>().cfill = fill;
            fill.transform.GetComponent<Image>().fillAmount += Time.deltaTime * cookSpeed;
            if (fill.transform.GetComponent<Image>().fillAmount >= 1)
            {
                cooked = true;
                overcooked = Time.time;
            }
        }

        if (holdingItem == true & cooked)
        {
            if (Time.time - overcooked >= burnTime)
            {
                cooked = false;
                Destroy(gameObject.transform.GetChild(0).gameObject);
                Instantiate(BurntMeatPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                fill.transform.GetComponent<Image>().color = bad;
                burnt = true;
                stop = true;
            }
        }

        if (cooked & stop == false)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
            Instantiate(CookedMeatPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            stop = true;
        }


        if (doomsday == true)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
            holdingItem = false;
            burnt = false;
            cooked = false;
            mask.SetActive(false);
            fill.transform.GetComponent<Image>().fillAmount = 0;
            fill.transform.GetComponent<Image>().color = Color.green;
            stop = false;
            doomsday = false;
        }

        if (holdingItem == false & transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
    }
}