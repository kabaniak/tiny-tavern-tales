using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player1control : MonoBehaviour
{
    public float speed = 11;
    public float prepSpeed = 0.1f;
    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    public bool carrying;
    public bool inRangeKeg;
    public bool inRangeRack;
    public bool inRangeDog;
    public bool inRangePrep;
    public bool inRangeHeat;
    public string currentObject = "";
    public GameObject Prep;
    public bool canServe;
    public Table servable;
    public GameObject pfill;

    // Start is called before the first frame update
    void Start()
    {
        carrying = false;
        inRangeKeg = false;
        inRangeRack = false;
        inRangeDog = false;
        inRangeHeat = false;
        inRangePrep = false;
        Prep = GameObject.FindWithTag("Prep");
        pfill = GameObject.Find("PrepFill");
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

        if (canServe & Input.GetKeyDown(KeyCode.E) & carrying == true)
        {
            servable.serveSeat(gameObject);
        }

        if (inRangeDog == true & Input.GetKeyDown(KeyCode.E) & carrying == true)
        {
            FeedtheDog();
        }

        if (inRangePrep == true & Input.GetKeyDown(KeyCode.E) & carrying == true & currentObject == "Meat")
        {
            PlaceOnSource(Prep, MeatPrefab);
            FeedtheDog();
            Prep.GetComponent<prepStation>().holdingItem = true;
        }

        if (Prep.GetComponent<prepStation>().holdingItem == true &
            Prep.GetComponent<prepStation>().prepComplete == false &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            PrepItem();
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
        if (collision.gameObject.name.Equals("TrashDog"))
        {
            inRangeDog = true;
        }
        if (collision.gameObject.name.Equals("PrepStation"))
        {
            inRangePrep = true;
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
        if (collision.gameObject.name.Equals("PrepStation"))
        {
            inRangePrep = false;
        }
    }

    public void FeedtheDog()
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);
        carrying = false;
        currentObject = "";
    }

    public void PlaceOnSource(GameObject source, GameObject item)
    {
        Instantiate(item, source.transform.position, Quaternion.identity, source.transform);
    }

    public void PrepItem()
    {
        pfill.transform.GetComponent<Image>().fillAmount += 0.1f; 
    }
}
