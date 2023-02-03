using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 10;


    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
