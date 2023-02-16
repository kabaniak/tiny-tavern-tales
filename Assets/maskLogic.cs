using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class maskLogic : MonoBehaviour
{
    public GameObject prep;
    public GameObject cook;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
