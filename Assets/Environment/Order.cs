using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{

    void Start()
    {

    }

    public void DisplayOrder(GameObject FoodPrefab, GameObject FacePrefab, Color npcColor)
    {
        GameObject food = Instantiate(FoodPrefab, transform.position + new Vector3(2, 0, 0), Quaternion.identity, transform);
        food.transform.localScale = new Vector3(8, 8, 1);
        GameObject face = Instantiate(FacePrefab, transform.position + new Vector3(-2, 0, 0), Quaternion.identity, transform);
        face.transform.localScale = new Vector3(1.56f, 0.6f, 1);
        face.GetComponent<SpriteRenderer>().color = npcColor;
    }
}
