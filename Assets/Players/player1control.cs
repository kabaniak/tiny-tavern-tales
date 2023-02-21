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

    //sprites
    public Sprite normalsprite;
    public Sprite b_sprite;
    public Sprite upm_sprite;
    public Sprite ucm_sprite;
    public Sprite cm_sprite;
    public Sprite bm_sprite;

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
        pfill = GameObject.Find("PrepFill");
        pmask = GameObject.Find("PrepMask");
        cfill = GameObject.Find("CookFill");
        cmask = GameObject.Find("CookMask");
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
            GetComponent<SpriteRenderer>().sprite = b_sprite;
        }

        // Get meat from rack
        if (inRangeRack == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            pickupFromSource(gameObject, MeatPrefab);
            carrying = true;
            currentObject = "Meat";
            GetComponent<SpriteRenderer>().sprite = upm_sprite;
        }

        // Serve Table
        if (canServe & Input.GetKeyDown(KeyCode.E))
        {
            servable.serveSeat(gameObject);
            GetComponent<SpriteRenderer>().sprite = normalsprite;
        }

        // Feed the Dog
        if (inRangeDog == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == true)
        {
            FeedtheDog();
            GetComponent<SpriteRenderer>().sprite = normalsprite;
            FindObjectOfType<TrashDog>().GetComponent<SpriteRenderer>().sprite = FindObjectOfType<TrashDog>().heart_sprite;
        }

        // Place meat onto the prep station
        if (inRangePrep == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == true &
            currentObject == "Meat" &
            Prep.GetComponent<prepStation>().holdingItem == false)
        {
            pmask.transform.GetComponent<Image>().color += new Color(0,0,0,1);
            PlaceOnSource(Prep, MeatPrefab);
            FeedtheDog();
            Prep.GetComponent<prepStation>().holdingItem = true;
            carrying = false;
            GetComponent<SpriteRenderer>().sprite = normalsprite;
        }

        // Prep the meat
        if (inRangePrep == true & 
            Prep.GetComponent<prepStation>().holdingItem == true &
            Prep.GetComponent<prepStation>().prepComplete == false &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            PrepItem();
        }

        // Pick up prepped meat
        if (Prep.GetComponent<prepStation>().holdingItem == true &
            Prep.GetComponent<prepStation>().prepComplete == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            pmask.transform.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
            pickupFromSource(gameObject, PreppedMeatPrefab);
            currentObject = "PreppedMeat";
            GetComponent<SpriteRenderer>().sprite = ucm_sprite;
        }

        // Place prepped meat onto cooking station
        if (inRangeHeat == true &
            Input.GetKeyDown(KeyCode.E) &
            carrying == true &
            currentObject == "PreppedMeat" &
            Cook.GetComponent<cookStation>().holdingItem == false)
        {
            cmask.transform.GetComponent<Image>().color += new Color(0, 0, 0, 1);
            PlaceOnSource(Cook, PreppedMeatPrefab);
            FeedtheDog();
            Cook.GetComponent<cookStation>().holdingItem = true;
            carrying = false;
            GetComponent<SpriteRenderer>().sprite = normalsprite;
        }

        // Pick up cooked or burnt meat from station
        if (inRangeHeat == true &
            Cook.GetComponent<cookStation>().holdingItem == true &
            (Cook.GetComponent<cookStation>().cooked == true | 
            Cook.GetComponent<cookStation>().burnt == true) &
            Input.GetKeyDown(KeyCode.E) &
            carrying == false)
        {
            cmask.transform.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
            if (Cook.GetComponent<cookStation>().cooked ==  true)
            {
                pickupFromSource(gameObject, CookedMeatPrefab);
                currentObject = "CookedMeat";
                GetComponent<SpriteRenderer>().sprite = cm_sprite;
            }
            else
            {
                pickupFromSource(gameObject, BurntMeatPrefab);
                currentObject = "BurntMeat";
                GetComponent<SpriteRenderer>().sprite = bm_sprite;
            }
            Cook.GetComponent<cookStation>().doomsday = true;
        }
    }

    public void pickupFromSource(GameObject player, GameObject source)
    {
        //GameObject created = Instantiate(source, player.transform.position, Quaternion.identity, player.transform);
        //created.GetComponent<SpriteRenderer>().sortingOrder = 3;
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
        //Destroy(gameObject.transform.GetChild(0).gameObject);
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
        if (pfill.transform.GetComponent<Image>().fillAmount >= 1)
        {
            Prep.transform.GetComponent<prepStation>().prepComplete = true;
        }
        pfill.transform.GetComponent<Image>().fillAmount += prepSpeed;
    }
}
