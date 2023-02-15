using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2control : MonoBehaviour
{
    public float speed = 11;
    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    private bool carrying;
    private bool inRangeKeg;
    private bool inRangeRack;
    public bool inRangeDog;
    public string currentObject = "";

    public bool canServe;
    public Table servable;

    // Start is called before the first frame update
    void Start()
    {
        carrying = false;
        inRangeRack = false;
        inRangeDog = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        var hmove2 = Input.GetAxis("Horizontal2");
        var vmove2 = Input.GetAxis("Vertical2");
        
        transform.GetComponent<Rigidbody2D>().position += new Vector2(hmove2, vmove2) * speed * Time.deltaTime;
        

        if (inRangeKeg == true & Input.GetKeyDown(KeyCode.Slash) & carrying == false)
        {
            pickupFromSource(gameObject, BoozePrefab);
            carrying = true;
            currentObject = "Booze";
        }


        if (inRangeRack == true & Input.GetKeyDown(KeyCode.Slash) & carrying == false)
        {
            pickupFromSource(gameObject, MeatPrefab);
            carrying = true;
            currentObject = "Meat";
        }

        if (canServe & Input.GetKeyDown(KeyCode.Slash))
        {
            servable.serveSeat(gameObject);
        }
        if (inRangeDog == true & Input.GetKeyDown(KeyCode.Slash) & carrying == true)
        {
            FeedtheDog();
        }
    }

    public void pickupFromSource(GameObject player, GameObject source)
    {
        GameObject created = Instantiate(source, player.transform.position, Quaternion.identity, player.transform);
        created.GetComponent<SpriteRenderer>().sortingOrder = 3;
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
        if (collision.gameObject.name.Equals("TrashDog"))
        {
            inRangeDog = true;
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
        if (collision.gameObject.name.Equals("TrashDog"))
        {
            inRangeDog = false;
        }
    }

    public void FeedtheDog()
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);
        carrying = false;
        currentObject = "";
    }
}
