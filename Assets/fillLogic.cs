using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fillLogic : MonoBehaviour
{
    public float fillAmount;
    public GameObject p1;
    public GameObject p2;
    public GameObject prep;
    public GameObject cook;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetComponent<Image>().fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
