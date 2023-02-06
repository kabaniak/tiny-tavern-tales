using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1control : MonoBehaviour
{
    public float speed = 10;
    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    private bool carrying;
    private bool inRangeKeg;
    private bool inRangeRack;
    public string currentObject = "";

    // Start is called before the first frame update
    void Start()
    {
        carrying = false;
        inRangeRack = false;
        inRangeRack = false;
    }

    // Update is called once per frame
    void Update()
    {
        var hmove1 = Input.GetAxis("Horizontal");
        var vmove1 = Input.GetAxis("Vertical");
        
        transform.position += new Vector3(hmove1, vmove1, 0) * speed * Time.deltaTime;
        

        if (inRangeKeg == true & Input.GetKeyDown(KeyCode.E) & carrying == false)
        {
            pickupFromSource(gameObject, BoozePrefab);
            carrying = true;
            currentObject = "Booze";
        }

        if (inRangeRack == true & Input.GetKeyDown(KeyCode.E) & carrying == false)
        {
            pickupFromSource(gameObject, MeatPrefab);
            carrying = true;
            currentObject = "Meat";
        }
    }

    void pickupFromSource(GameObject player, GameObject source)
    {
        Instantiate(source, player.transform.position, Quaternion.identity, player.transform);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("MeatStorage"))
        {
            inRangeRack = true;
        }
        if (collision.gameObject.name.Equals("Kegs"))
        {
            inRangeKeg = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("MeatStorage"))
        {
            inRangeRack = false;
        }

        if (collision.gameObject.name.Equals("Kegs"))
        {
            inRangeKeg = false;
        }
    }
}
