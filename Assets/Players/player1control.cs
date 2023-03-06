using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player1control : MonoBehaviour
{
    // floats
    public float speed = 11;
    public float prepSpeed = 0.05f;
    private float placeTime;

    // prefabs
    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    public GameObject PreppedMeatPrefab;
    public GameObject CookedMeatPrefab;
    public GameObject BurntMeatPrefab;

    // bools (optimize later)
    public bool carrying;
    public GameObject inRangeBrawl = null;
    public List<GameObject> inRangeMess = new List<GameObject>();
    public bool inRangeKeg;
    public bool inRangeRack;
    public bool inRangeDog;
    public bool inRangePrep;
    public bool inRangeHeat;
    public bool inRangeCounter1;
    public bool inRangeCounter2;
    public bool inRangeCounter3;
    public bool canServe;

    // holders
    public string currentObject = "";
    private bool holdKeg;
    private bool holdRack;
    private bool holdDog;
    private bool holdPrep;
    private bool holdHeat;
    private bool holdCounter1;
    private bool holdCounter2;


    // interactables
    public GameObject Prep;
    public GameObject Cook;
    public GameObject holding1;
    public GameObject holding2;
    public GameObject holding3;
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
        inRangeCounter1 = false;
        inRangeCounter2 = false;
        Prep = GameObject.FindWithTag("Prep");
        Cook = GameObject.FindWithTag("Heat");
        pfill = GameObject.Find("PrepFill");
        pmask = GameObject.Find("PrepMask");
        cfill = GameObject.Find("CookFill");
        cmask = GameObject.Find("CookMask");
        holding1 = GameObject.Find("Counter1");
        holding2 = GameObject.Find("Counter2");
        holding3 = GameObject.Find("Counter3");
    }

    // Update is called once per frame
    void Update()
    {
        // Move Player
        var hmove1 = Input.GetAxis("Horizontal");
        var vmove1 = Input.GetAxis("Vertical");
        transform.GetComponent<Rigidbody2D>().position += new Vector2(hmove1, vmove1) * speed * Time.deltaTime;

        var interacting = Input.GetKeyDown(KeyCode.E);
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

        //Pickup from counters
        if (inRangeCounter1 &
            interacting &
            carrying == true &
            holding1.GetComponent<holdingStationLogic>().holding == false)
        {
            placeTime = Time.time;
            holding1.GetComponent<holdingStationLogic>().holding = true;
            if (currentObject == "Meat")
            {
                PlaceOnSource(holding1, MeatPrefab);
                holding1.GetComponent<holdingStationLogic>().currentObject = "Meat";
            }
            else if (currentObject == "BurntMeat")
            {
                PlaceOnSource(holding1, BurntMeatPrefab);
                holding1.GetComponent<holdingStationLogic>().currentObject = "BurntMeat";
            }
            else if (currentObject == "PreppedMeat")
            {
                PlaceOnSource(holding1, PreppedMeatPrefab);
                holding1.GetComponent<holdingStationLogic>().currentObject = "PreppedMeat";
            }
            else if (currentObject == "CookedMeat")
            {
                PlaceOnSource(holding1, CookedMeatPrefab);
                holding1.GetComponent<holdingStationLogic>().currentObject = "CookedMeat";
            }
            else if (currentObject == "Booze")
            {
                PlaceOnSource(holding1, BoozePrefab);
                holding1.GetComponent<holdingStationLogic>().currentObject = "Booze";
            }
            FeedtheDog();
            carrying = false;
            didSomething = true;
        }
        else if (inRangeCounter2 &
           interacting &
           carrying == true &
           holding2.GetComponent<holdingStationLogic>().holding == false)
        {
            placeTime = Time.time;
            holding2.GetComponent<holdingStationLogic>().holding = true;
            if (currentObject == "Meat")
            {
                PlaceOnSource(holding2, MeatPrefab);
                holding2.GetComponent<holdingStationLogic>().currentObject = "Meat";
            }
            else if (currentObject == "BurntMeat")
            {
                PlaceOnSource(holding2, BurntMeatPrefab);
                holding2.GetComponent<holdingStationLogic>().currentObject = "BurntMeat";
            }
            else if (currentObject == "PreppedMeat")
            {
                PlaceOnSource(holding2, PreppedMeatPrefab);
                holding2.GetComponent<holdingStationLogic>().currentObject = "PreppedMeat";
            }
            else if (currentObject == "CookedMeat")
            {
                PlaceOnSource(holding2, CookedMeatPrefab);
                holding2.GetComponent<holdingStationLogic>().currentObject = "CookedMeat";
            }
            else if (currentObject == "Booze")
            {
                PlaceOnSource(holding2, BoozePrefab);
                holding2.GetComponent<holdingStationLogic>().currentObject = "Booze";
            }
            FeedtheDog();
            carrying = false;
            didSomething = true;
        }
        else if (inRangeCounter3 &
          interacting &
          carrying == true &
          holding3.GetComponent<holdingStationLogic>().holding == false)
        {
            placeTime = Time.time;
            holding3.GetComponent<holdingStationLogic>().holding = true;
            if (currentObject == "Meat")
            {
                PlaceOnSource(holding3, MeatPrefab);
                holding3.GetComponent<holdingStationLogic>().currentObject = "Meat";
            }
            else if (currentObject == "BurntMeat")
            {
                PlaceOnSource(holding3, BurntMeatPrefab);
                holding3.GetComponent<holdingStationLogic>().currentObject = "BurntMeat";
            }
            else if (currentObject == "PreppedMeat")
            {
                PlaceOnSource(holding3, PreppedMeatPrefab);
                holding3.GetComponent<holdingStationLogic>().currentObject = "PreppedMeat";
            }
            else if (currentObject == "CookedMeat")
            {
                PlaceOnSource(holding3, CookedMeatPrefab);
                holding3.GetComponent<holdingStationLogic>().currentObject = "CookedMeat";
            }
            else if (currentObject == "Booze")
            {
                PlaceOnSource(holding3, BoozePrefab);
                holding3.GetComponent<holdingStationLogic>().currentObject = "Booze";
            }
            FeedtheDog();
            carrying = false;
            didSomething = true;
        }

        // place on counter
        if (inRangeCounter1 &&
           interacting &&
           carrying == false &&
           currentObject == "" &&
           holding1.GetComponent<holdingStationLogic>().holding == true &
           Time.time - placeTime > 0.1)
        {
            if (holding1.GetComponent<holdingStationLogic>().currentObject == "Meat")
            {
                currentObject = "Meat";
            }
            else if (holding1.GetComponent<holdingStationLogic>().currentObject == "BurntMeat")
            {
                currentObject = "BurntMeat";
            }
            else if (holding1.GetComponent<holdingStationLogic>().currentObject == "PreppedMeat")
            {
                currentObject = "PreppedMeat";
            }
            else if (holding1.GetComponent<holdingStationLogic>().currentObject == "CookedMeat")
            {
                currentObject = "CookedMeat";
            }
            else if (holding1.GetComponent<holdingStationLogic>().currentObject == "Booze")
            {
                currentObject = "Booze";
            }
            carrying = true;
            holding1.GetComponent<holdingStationLogic>().holding = false;
            didSomething = true;
        }
        else if (inRangeCounter2 &
           carrying == false &
           holding2.GetComponent<holdingStationLogic>().holding == true &
           interacting &
           Time.time - placeTime > 0.1)
        {
            if (holding2.GetComponent<holdingStationLogic>().currentObject == "Meat")
            {
                currentObject = "Meat";
            }
            else if (holding2.GetComponent<holdingStationLogic>().currentObject == "BurntMeat")
            {
                currentObject = "BurntMeat";
            }
            else if (holding2.GetComponent<holdingStationLogic>().currentObject == "PreppedMeat")
            {
                currentObject = "PreppedMeat";
            }
            else if (holding2.GetComponent<holdingStationLogic>().currentObject == "CookedMeat")
            {
                currentObject = "CookedMeat";
            }
            else if (holding2.GetComponent<holdingStationLogic>().currentObject == "Booze")
            {
                currentObject = "Booze";
            }
            holding2.GetComponent<holdingStationLogic>().holding = false;
            carrying = true;
            didSomething = true;
        }
        else if (inRangeCounter3 &
          carrying == false &
          holding3.GetComponent<holdingStationLogic>().holding == true &
          interacting &
          Time.time - placeTime > 0.1)
        {
            if (holding3.GetComponent<holdingStationLogic>().currentObject == "Meat")
            {
                currentObject = "Meat";
            }
            else if (holding3.GetComponent<holdingStationLogic>().currentObject == "BurntMeat")
            {
                currentObject = "BurntMeat";
            }
            else if (holding3.GetComponent<holdingStationLogic>().currentObject == "PreppedMeat")
            {
                currentObject = "PreppedMeat";
            }
            else if (holding3.GetComponent<holdingStationLogic>().currentObject == "CookedMeat")
            {
                currentObject = "CookedMeat";
            }
            else if (holding3.GetComponent<holdingStationLogic>().currentObject == "Booze")
            {
                currentObject = "Booze";
            }
            holding3.GetComponent<holdingStationLogic>().holding = false;
            carrying = true;
            didSomething = true;
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
            pmask.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);

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
            pmask.transform.GetComponent<Image>().color = new Color(1, 1, 1, 0); 
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
            cmask.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1); 
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
            cmask.transform.GetComponent<Image>().color = new Color(1, 1, 1, 0); 
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
            didSomething = true;
        }

        // if didn't do anything else
        if(!didSomething && inRangeMess.Count != 0 && interacting)
        {
            Destroy(inRangeMess[0]);
        }

        if (didSomething)
        {
            updateSpriteByCurrObject();
        }
    }

    public void pickupFromSource(GameObject player, GameObject source)
    {
        //GameObject created = Instantiate(source, player.transform.position, Quaternion.identity, player.transform);
        //created.GetComponent<SpriteRenderer>().sortingOrder = 3;
        carrying = true;
    }

    public void updateSpriteByCurrObject()
    {
        if(currentObject == "CookedMeat")
        {
            GetComponent<SpriteRenderer>().sprite = cm_sprite;
        }
        else if(currentObject == "BurntMeat")
        {
            GetComponent<SpriteRenderer>().sprite = bm_sprite;
        }
        else if(currentObject == "PreppedMeat")
        {
            GetComponent<SpriteRenderer>().sprite = ucm_sprite;
        }
        else if(currentObject == "Meat")
        {
            GetComponent<SpriteRenderer>().sprite = upm_sprite;
        }
        else if(currentObject == "Booze")
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
            inRangeMess.Add(collision.gameObject);
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
        if (collision.gameObject.name.Equals("Counter1"))
        {
            inRangeCounter1 = true;
        }
        if (collision.gameObject.name.Equals("Counter2"))
        {
            inRangeCounter2 = true;
        }
        if (collision.gameObject.name.Equals("Counter3"))
        {
            inRangeCounter3 = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Mess"))
        {
            inRangeMess.Remove(collision.gameObject);
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
        if (collision.gameObject.name.Equals("Counter1"))
        {
            inRangeCounter1 = false;
        }
        if (collision.gameObject.name.Equals("Counter2"))
        {
            inRangeCounter2 = false;
        }
        if (collision.gameObject.name.Equals("Counter3"))
        {
            inRangeCounter3 = false;
        }
    }

    public void FeedtheDog()
    {
        holdRack = inRangeRack;
        holdKeg = inRangeKeg;
        holdDog = inRangeDog;
        holdPrep = inRangePrep;
        holdHeat = inRangeHeat;
        holdCounter1 = inRangeCounter1;
        holdCounter2 = inRangeCounter2;
        //Destroy(gameObject.transform.GetChild(0).gameObject);
        carrying = false;
        currentObject = "";
        inRangeRack = holdRack;
        inRangeKeg = holdKeg;
        inRangeDog = holdDog;
        inRangePrep = holdPrep;
        inRangeHeat = holdHeat;
        inRangeCounter1 = holdCounter1;
        inRangeCounter2 = holdCounter2;
    }

    public void PlaceOnSource(GameObject source, GameObject item)
    {
        if ((source == holding1 | source == holding2 | source==holding3) & currentObject == "Booze")
        {
            GameObject noscale = Instantiate(item, source.transform.position, Quaternion.identity, source.transform);
            noscale.transform.localScale = new Vector3(0.75f, 3f, 1f);
        }
        else if ((source == holding1 | source == holding2 | source == holding3) & (currentObject == "Meat" | currentObject == "PreppedMeat"))
        {
            GameObject noscale = Instantiate(item, source.transform.position, Quaternion.identity, source.transform);
            noscale.transform.localScale = new Vector3(0.75f, 3f, 1f);
        }
        else if ((source == holding1 | source == holding2 | source == holding3) & (currentObject == "CookedMeat" | currentObject == "BurntMeat"))
        {
            GameObject noscale = Instantiate(item, source.transform.position, Quaternion.identity, source.transform);
            noscale.transform.localScale = new Vector3(0.75f, 3f, 1f);
        }
        else if (source == Prep & currentObject == "Meat")
        {
            GameObject noscale = Instantiate(item, source.transform.position, Quaternion.identity, source.transform);
            noscale.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
        }
        else if (source == Cook & currentObject == "PreppedMeat")
        {
            GameObject noscale = Instantiate(item, source.transform.position, Quaternion.identity, source.transform);
            noscale.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
        }
        else
        {
            Instantiate(item, source.transform.position, Quaternion.identity, source.transform);
        }
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
