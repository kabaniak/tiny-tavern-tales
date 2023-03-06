using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class numToStr : MonoBehaviour
{
    public int value;
    public float valuef;
    public Text valueText;
    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent.tag == "reportFloat")
        {
            valueText.text = valuef.ToString("F1");
        }
        else
        {
            valueText.text = value.ToString();
        }

        // Correct Reporting Numbers
        if (gameObject.transform.parent == GameObject.Find("RatingStat").transform)
        {
            valuef = gameManager.GetComponent<GameManager>().reputation / 5f;
        }
        else if (gameObject.transform.parent == GameObject.Find("GoldStat").transform)
        {
            value = gameManager.GetComponent<GameManager>().coinsToday;
        }
        else if (gameObject.transform.parent == GameObject.Find("ServedStat").transform)
        {
            value = gameManager.GetComponent<GameManager>().servedToday;
        }
        else if (gameObject.transform.parent == GameObject.Find("BrawlStat").transform)
        {
            value = gameManager.GetComponent<GameManager>().brawlsToday;
        }
        else if (gameObject.transform.parent == GameObject.Find("UnsatisfiedStat").transform)
        {
            value = gameManager.GetComponent<GameManager>().angryToday;
        }
        else if (gameObject.transform.parent == GameObject.Find("fRatingStat").transform)
        {
            valuef = gameManager.GetComponent<GameManager>().reputation / 5f;
        }
        else if (gameObject.transform.parent == GameObject.Find("fGoldStat").transform)
        {
            value = gameManager.GetComponent<GameManager>().totalCoins;
        }
        else if (gameObject.transform.parent == GameObject.Find("fServedStat").transform)
        {
            value = gameManager.GetComponent<GameManager>().servedFinal;
        }
        else if (gameObject.transform.parent == GameObject.Find("fBrawlStat").transform)
        {
            value = gameManager.GetComponent<GameManager>().brawlsFinal;
        }
        else if (gameObject.transform.parent == GameObject.Find("fUnsatisfiedStat").transform)
        {
            value = gameManager.GetComponent<GameManager>().angryFinal;
        }
    }
}
