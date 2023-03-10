using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderTracker : MonoBehaviour
{

    GameObject[] npcOrderInfo = { null, null, null, null, null, null, null, null};
    GameObject[] orderDisplay = { null, null, null, null, null, null, null, null };

    public GameObject OrderPrefab;

    public GameObject BoozePrefab;
    public GameObject MeatPrefab;
    public GameObject FacePrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // sort by how much patience is left (come back)
        sortByPatience();

        // display orders
        for (int i = 0; i < npcOrderInfo.Length; i++)
        {
            if (npcOrderInfo[i] != null && orderDisplay[i] == null)
            {
                // instantiate the order in the tracker
                float xPos = 2;
                float yPos = 15 - 4 * i;
                orderDisplay[i]=Instantiate(OrderPrefab, transform.position + new Vector3(xPos, yPos, 0), Quaternion.identity, transform);
                int npcSpriteNum = npcOrderInfo[i].GetComponent<NPCSpriteBehavior>().getSpriteNum();
                string npcSpriteType = npcOrderInfo[i].GetComponent<NPCSpriteBehavior>().getSpriteType();
                if (npcOrderInfo[i].GetComponent<NPCSpriteBehavior>().getMyOrder() == "Booze")
                {
                    orderDisplay[i].GetComponent<Order>().DisplayOrder(BoozePrefab, FacePrefab, npcSpriteNum, npcSpriteType);
                }
                else
                {
                    orderDisplay[i].GetComponent<Order>().DisplayOrder(MeatPrefab, FacePrefab, npcSpriteNum, npcSpriteType);
                }
            }
            if(npcOrderInfo[i] != null && orderDisplay[i] != null)
            {
                orderDisplay[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = npcOrderInfo[i].GetComponent<SpriteRenderer>().color;
            }
        }
    }
    public void addOrder(GameObject npc)
    {
        for (int i = 0; i< npcOrderInfo.Length; i++)
        {
            if (npcOrderInfo[i] == null)
            {
                npcOrderInfo[i] = npc;
                return;
            }
        }
    }

    public void removeMyOrder(GameObject npc)
    {
        for (int i = 0; i < npcOrderInfo.Length; i++)
        {
            if (npcOrderInfo[i] == npc)
            {
                removeOrder(i);
                return;
            }
        }
    }

    private void removeOrder(int index)
    {

        npcOrderInfo[index] = null;
        Destroy(orderDisplay[index]);
        orderDisplay[index] = null;
        for (int i = index + 1; i< npcOrderInfo.Length; i++)
        {
            npcOrderInfo[i - 1] = npcOrderInfo[i];
            Destroy(orderDisplay[i]);
            orderDisplay[i] = null;
        }

        npcOrderInfo[7] = null;

    }

    private void sortByPatience()
    {
        for (int i = 0; i < npcOrderInfo.Length - 1; i++)
        {
            for (int j = 0; j < npcOrderInfo.Length - i - 1; j++)
            {
                if (npcOrderInfo[j] == null)
                {
                    if (npcOrderInfo[j+1] == null)
                    {
                        continue;
                    }
                    else
                    {
                        // swap temp and arr[i]
                        GameObject temp = npcOrderInfo[j];
                        npcOrderInfo[j] = npcOrderInfo[j + 1];
                        npcOrderInfo[j + 1] = temp;

                        Destroy(orderDisplay[j + 1]);
                        orderDisplay[j] = null;
                        orderDisplay[j + 1] = null;
                    }
                }
                if (npcOrderInfo[j + 1] == null)
                {
                    continue;
                }

                NPCSpriteBehavior one = npcOrderInfo[j].GetComponent<NPCSpriteBehavior>();
                NPCSpriteBehavior two = npcOrderInfo[j + 1].GetComponent<NPCSpriteBehavior>();
                if (one.patience > two.patience)
                {
                    // swap temp and arr[i]
                    GameObject temp = npcOrderInfo[j];
                    npcOrderInfo[j] = npcOrderInfo[j + 1];
                    npcOrderInfo[j + 1] = temp;

                    // destroy the displays since we swapped
                    Destroy(orderDisplay[j]);
                    Destroy(orderDisplay[j+1]);
                    orderDisplay[j] = null;
                    orderDisplay[j + 1] = null;
                }
            }
        }
    }
}
