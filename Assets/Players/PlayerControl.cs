using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 10;
    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    private bool carrying;
    private bool inRangeKeg;
    private bool inRangeRack;
    public GameObject p1;
    public GameObject p2;
    public string currentObject = "";

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.FindWithTag("Player1");
        p2 = GameObject.FindWithTag("Player2");
        carrying = false;
        inRangeRack = false;
        inRangeRack = false;
    }

    // Update is called once per frame
    void Update()
    {
        var hmove1 = Input.GetAxis("Horizontal");
        var vmove1 = Input.GetAxis("Vertical");
        var hmove2 = Input.GetAxis("Horizontal2");
        var vmove2 = Input.GetAxis("Vertical2");
        if (gameObject.tag == "Player1")
        {
            transform.position += new Vector3(hmove1, vmove1, 0) * speed * Time.deltaTime;
        }

        if (gameObject.tag == "Player2")
        {
            transform.position += new Vector3(hmove2, vmove2, 0) * speed * Time.deltaTime;
        }

        if (inRangeKeg == true & Input.GetKeyDown(KeyCode.E) & carrying == false)
        {
            pickupFromSource(p1, BoozePrefab);
            carrying = true;
            currentObject = "Booze";
        }


        if (inRangeKeg == true & Input.GetKeyDown(KeyCode.Slash) & carrying == false)
        {
            pickupFromSource(p2, BoozePrefab);
            carrying = true;
            currentObject = "Booze";
        }

        if (inRangeRack == true & Input.GetKeyDown(KeyCode.E) & carrying == false)
        {
            pickupFromSource(p1, MeatPrefab);
            carrying = true;
            currentObject = "Meat";
        }


        if (inRangeRack == true & Input.GetKeyDown(KeyCode.Slash) & carrying == false)
        {
            pickupFromSource(p2, MeatPrefab);
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
