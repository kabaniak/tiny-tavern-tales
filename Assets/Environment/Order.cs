using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    //face sprites
    public List<Sprite> HumanFaces;
    public List<Sprite> TieflingFaces;
    public List<Sprite> ElfFaces;

    void Start()
    {

    }

    public void DisplayOrder(GameObject FoodPrefab, GameObject FacePrefab, int SpriteNum, string SpriteType)
    {
        GameObject food = Instantiate(FoodPrefab, transform.position + new Vector3(2, 0, 0), Quaternion.identity, transform);
        //food.transform.localScale = new Vector3(8, 8, 1);
        GameObject face = Instantiate(FacePrefab, transform.position + new Vector3(-2, 0, 0), Quaternion.identity, transform);
        //face.transform.localScale = new Vector3(1.56f, 0.6f, 1);
        if (SpriteType == "HumanFighter")
        {
            face.GetComponent<SpriteRenderer>().sprite = HumanFaces[SpriteNum];
        }
        else if (SpriteType == "TieflingSorcerer")
        {
            face.GetComponent<SpriteRenderer>().sprite = TieflingFaces[SpriteNum];
        }
        else if (SpriteType == "ElfRogue")
        {
            face.GetComponent<SpriteRenderer>().sprite = ElfFaces[SpriteNum];
        }
   
    }
}
