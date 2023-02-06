using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kegs : MonoBehaviour
{
    private bool pickupable = false;
    public GameObject BoozePrefab;
    public GameObject p1;
    public GameObject p2;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.FindWithTag("Player1");
        p2 = GameObject.FindWithTag("Player2");
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (pickupable == true && Input.GetKeyDown(KeyCode.E))
    //    {
    //        Pickup(p1);
    //    }

    //    if (pickupable == true && Input.GetKeyDown(KeyCode.Slash))
    //    {
    //       Pickup(p2);
    //    }
    //}

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name.Equals("p1") | collision.gameObject.name.Equals("p2"))
    //    {
    //        pickupable = true;
    //    }
    //}

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name.Equals("p1") | collision.gameObject.name.Equals("p2"))
    //    {
    //        pickupable = false;
    //    }
    //}

    //void Pickup(GameObject player)
    //{
    //    Instantiate(BoozePrefab, player.transform.position, Quaternion.identity, player.transform);
    //}
}
