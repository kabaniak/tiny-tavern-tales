using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdingStationLogic : MonoBehaviour
{
    public bool holding;
    public string currentObject;

    public GameObject MeatPrefab;
    public GameObject PreppedMeatPrefab;
    public GameObject CookedMeatPrefab;
    public GameObject BurntMeatPrefab;
    // Start is called before the first frame update
    void Start()
    {
        holding = false;
        currentObject = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (holding == false & transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
    }

    public void clearCounter()
    {
        holding = false;
        currentObject = "";
        if(gameObject.transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
    }
}
