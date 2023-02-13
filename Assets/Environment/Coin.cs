using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    float lifespan = 15;
    public GameObject myTable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lifespan <= 0)
        {
            myTable.GetComponent<Table>().removeCoin(gameObject);
            Destroy(gameObject);
        }

        lifespan -= 0.25f;
    }
}
