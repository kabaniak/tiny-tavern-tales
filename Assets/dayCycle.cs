using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dayCycle : MonoBehaviour
{
    private float timeRate = 0.005f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetComponent<Image>().fillAmount = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {  
        if (gameObject.transform.GetComponent<Image>().fillAmount <= 1f)
        {
            gameObject.transform.GetComponent<Image>().fillAmount += timeRate * Time.deltaTime;
        }
    }
}
