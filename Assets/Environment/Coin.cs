using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    float lifespan = 15;
    public GameObject myTable;

    public int[] value = { 0, 0 };

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

            GameObject.FindObjectOfType<GameManager>().orderCompleted(value[0], value[1]);

            Destroy(gameObject);
        }

        lifespan -= 0.25f;
    }
}
