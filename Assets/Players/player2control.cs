using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player2control : MonoBehaviour
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
    public GameObject inRangeBrawl = null;
    public GameObject inRangeMess = null;
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
    }

    // Update is called once per frame
    void Update()
    {
        // Move Player
        var hmove2 = Input.GetAxis("Horizontal2");
        var vmove2 = Input.GetAxis("Vertical2");
        transform.GetComponent<Rigidbody2D>().position += new Vector2(hmove2, vmove2) * speed * Time.deltaTime;

        var interacting = Input.GetKeyDown(KeyCode.Slash);
        bool didSomething = false;

        // If in range of a brawl can't interact with anything else
        if (inRangeBrawl != null & interacting)
        {
            // try to break up brawl
            return;
        }

        // Pick up beer from keg
        if (inRangeKeg == true &
            interacting &
            carrying == false)
        {
            pickupFromSource(gameObject, BoozePrefab);
            carrying = true;
            currentObject = "Booze";
            didSomething = true;
        }

        // Get meat from rack
        if (inRangeRack == true &
            interacting &
            carrying == false)
        {
            pickupFromSource(gameObject, MeatPrefab);
            carrying = true;
            currentObject = "Meat";
            didSomething = true;
        }

        // Serve Table
        if (canServe & interacting)
        {
            didSomething = servable.serveSeat(gameObject);
        }

        // Feed the Dog
        if (inRangeDog == true &
            interacting &
            carrying == true)
        {
            FeedtheDog();
            FindObjectOfType<TrashDog>().GetComponent<SpriteRenderer>().sprite = FindObjectOfType<TrashDog>().heart_sprite;
            didSomething = true;
        }

        // Place meat onto the prep station
        if (inRangePrep == true &
            interacting &
            carrying == true &
            currentObject == "Meat" &
            Prep.GetComponent<prepStation>().holdingItem == false)
        {
            pmask.transform.GetComponent<Image>().color += new Color(0, 0, 0, 1);
            PlaceOnSource(Prep, MeatPrefab);
            FeedtheDog();
            Prep.GetComponent<prepStation>().holdingItem = true;
            carrying = false;
            didSomething = true;
        }

        // Prep the meat
        if (inRangePrep == true &
            Prep.GetComponent<prepStation>().holdingItem == true &
            Prep.GetComponent<prepStation>().prepComplete == false &
            interacting &
            carrying == false)
        {
            PrepItem();
            didSomething = true;
        }

        // Pick up prepped meat
        if (Prep.GetComponent<prepStation>().holdingItem == true &
            Prep.GetComponent<prepStation>().prepComplete == true &
            interacting &
            carrying == false)
        {
            pmask.transform.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
            pickupFromSource(gameObject, PreppedMeatPrefab);
            currentObject = "PreppedMeat";
            didSomething = true;
        }

        // Place prepped meat onto cooking station
        if (inRangeHeat == true &
            interacting &
            carrying == true &
            currentObject == "PreppedMeat" &
            Cook.GetComponent<cookStation>().holdingItem == false)
        {
            cmask.transform.GetComponent<Image>().color += new Color(0, 0, 0, 1);
            PlaceOnSource(Cook, PreppedMeatPrefab);
            FeedtheDog();
            Cook.GetComponent<cookStation>().holdingItem = true;
            carrying = false;
            didSomething = true;
        }

        // Pick up cooked or burnt meat from station
        if (inRangeHeat == true &
            Cook.GetComponent<cookStation>().holdingItem == true &
            (Cook.GetComponent<cookStation>().cooked == true |
            Cook.GetComponent<cookStation>().burnt == true) &
            interacting &
            carrying == false)
        {
            cmask.transform.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
            if (Cook.GetComponent<cookStation>().cooked == true)
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
            didSomething = true;
        }

        // if didn't do anything else
        if (!didSomething && inRangeMess != null && interacting)
        {
            Destroy(inRangeMess);
            inRangeMess = null;
        }

        if (didSomething) { updateSpriteByCurrObject(); }
    }

    public void pickupFromSource(GameObject player, GameObject source)
    {
        //GameObject created = Instantiate(source, player.transform.position, Quaternion.identity, player.transform);
        //created.GetComponent<SpriteRenderer>().sortingOrder = 3;
        carrying = true;
    }

    public void updateSpriteByCurrObject()
    {
        if (currentObject == "CookedMeat")
        {
            GetComponent<SpriteRenderer>().sprite = cm_sprite;
        }
        else if (currentObject == "BurntMeat")
        {
            GetComponent<SpriteRenderer>().sprite = bm_sprite;
        }
        else if (currentObject == "PreppedMeat")
        {
            GetComponent<SpriteRenderer>().sprite = ucm_sprite;
        }
        else if (currentObject == "Meat")
        {
            GetComponent<SpriteRenderer>().sprite = upm_sprite;
        }
        else if (currentObject == "Booze")
        {
            GetComponent<SpriteRenderer>().sprite = b_sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = normalsprite;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Mess"))
        {
            inRangeMess = collision.gameObject;
        }
        if (collision.gameObject.tag.Equals("Brawl"))
        {
            inRangeBrawl = collision.gameObject;
        }
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
        if (collision.gameObject.tag.Equals("Mess"))
        {
            inRangeMess = null;
        }
        if (collision.gameObject.tag.Equals("Brawl"))
        {
            inRangeBrawl = null;
        }
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
