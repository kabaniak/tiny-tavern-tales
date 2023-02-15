using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player1control : MonoBehaviour
{
    // floats
    public float speed = 11;
    public float prepSpeed = 0.05f;

    // prefabs
    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    public GameObject PreppedMeatPrefab;
    public GameObject CookedMeatPrefab;
    public GameObject BurntMeatPrefab;

    // bools (optimize later)
    public bool carrying;
    public bool inRangeKeg;
    public bool inRangeRack;
    public bool inRangeDog;
    public bool inRangePrep;
    public bool inRangeHeat;
    public bool canServe;

    // holders
    public string currentObject = "";
    private bool holdKeg;
    private bool holdRack;
    private bool holdDog;
    private bool holdPrep;
    private bool holdHeat;

    // interatables
    public GameObject Prep;
    public GameObject Cook;
    public Table servable;

    // ui
    public GameObject pfill;
    public GameObject pmask;
    public GameObject cfill;
    public GameObject cmask;

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
        Cook = GameObject.FindWithTag("Heat");
        pmask = GameObject.Find("PrepMask");
        pfill = GameObject.Find("PrepFill");
        cmask = GameObject.Find("CookMask");
        cfill = GameObject.Find("CookFill");
        pmask.SetActive(false);
        cmask.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Move Player
        var hmove1 = Input.GetAxis("Horizontal");
        var vmove1 = Input.GetAxis("Vertical");
        transform.GetComponent<Rigidbody2D>().position += new Vector2(hmove1, vmove1) * speed * Time.deltaTime;

        // Pick up beer from keg
        if (inRangeKeg == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            pickupFromSource(gameObject, BoozePrefab);
            carrying = true;
            currentObject = "Booze";
        }

        // Get meat from rack
        if (inRangeRack == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            pickupFromSource(gameObject, MeatPrefab);
            carrying = true;
            currentObject = "Meat";
        }

        // Serve Table
        if (canServe & Input.GetKeyDown(KeyCode.E) &
            carrying == true)
        {
            servable.serveSeat(gameObject);
        }

        // Feed the Dog
        if (inRangeDog == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == true)
        {
            FeedtheDog();
        }

        // Place meat onto the prep station
        if (inRangePrep == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == true &
            currentObject == "Meat" &
            Prep.GetComponent<prepStation>().holdingItem == false)
        {
            PlaceOnSource(Prep, MeatPrefab);
            FeedtheDog();
            Prep.GetComponent<prepStation>().holdingItem = true;
            pfill = GameObject.Find("PrepFill");
            carrying = false;
        }

        // Prep the meat
        if (inRangePrep == true & 
            Prep.GetComponent<prepStation>().holdingItem == true &
            Prep.GetComponent<prepStation>().prepComplete == false &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            pmask.SetActive(true);
            PrepItem();
        }

        // Pick up prepped meat
        if (Prep.GetComponent<prepStation>().holdingItem == true &
            Prep.GetComponent<prepStation>().prepComplete == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            pickupFromSource(gameObject, PreppedMeatPrefab);
            currentObject = "PreppedMeat";
        }

        // Place prepped meat onto cooking station
        if (inRangeHeat == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == true &
            currentObject == "PreppedMeat" &
            Cook.GetComponent<cookStation>().holdingItem == false)
        {
            PlaceOnSource(Cook, PreppedMeatPrefab);
            FeedtheDog();
            Cook.GetComponent<cookStation>().holdingItem = true;
            carrying = false;
        }

        // Pick up cooked or burnt meat from station
        if (inRangeHeat == true &
            Cook.GetComponent<cookStation>().holdingItem == true &
            (Cook.GetComponent<cookStation>().cooked == true | 
            Cook.GetComponent<cookStation>().burnt == true) &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            if (Cook.GetComponent<cookStation>().cooked ==  true)
            {
                pickupFromSource(gameObject, CookedMeatPrefab);
                currentObject = "CookedMeat";
            }
            else
            {
                pickupFromSource(gameObject, BurntMeatPrefab);
                currentObject = "BurntMeat";
            }
            Cook.GetComponent<cookStation>().doomsday = true;
        }
    }

    void pickupFromSource(GameObject player, GameObject source)
    {
        GameObject created = Instantiate(source, player.transform.position, Quaternion.identity, player.transform);
        created.GetComponent<SpriteRenderer>().sortingOrder = 3;
        carrying = true;
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
        if (collision.gameObject.name.Equals("CookStation"))
        {
            inRangeHeat = true;
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
        if (collision.gameObject.name.Equals("CookStation"))
        {
            inRangeHeat = false;
        }
    }

    public void FeedtheDog()
    {
        holdRack = inRangeRack;
        holdKeg = inRangeKeg;
        holdDog = inRangeDog;
        holdPrep = inRangePrep;
        holdHeat = inRangeHeat;
        Destroy(gameObject.transform.GetChild(0).gameObject);
        carrying = false;
        currentObject = "";
        inRangeRack = holdRack;
        inRangeKeg = holdKeg;
        inRangeDog = holdDog;
        inRangePrep = holdPrep;
        inRangeHeat = holdHeat;
    }

    public void PlaceOnSource(GameObject source, GameObject item)
    {
        Instantiate(item, source.transform.position, Quaternion.identity, source.transform);
    }

    public void PrepItem()
    {
        pfill = GameObject.Find("PrepFill");
        if (pfill.transform.GetComponent<Image>().fillAmount >= 1)
        {
            Prep.transform.GetComponent<prepStation>().prepComplete = true;
            pmask.SetActive(false);
        }
        pfill.transform.GetComponent<Image>().fillAmount += prepSpeed;
    }
}
