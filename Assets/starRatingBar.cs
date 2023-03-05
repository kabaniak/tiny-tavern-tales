using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starRatingBar : MonoBehaviour
{
    public bool cleared = false;
    public GameObject starPrefab;
    public GameManager gameManager;
    public GameObject generator, p1, p2, cookMask, gameOver;
    List<starRating> stars = new List<starRating>();

    public void Start()
    {
        ClearStars();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        CreateStars();
        DrawStars();
        generator = GameObject.Find("NPC Generator");
        p1 = GameObject.Find("p1");
        p2 = GameObject.Find("p2");
        cookMask = GameObject.Find("CookMask");
        gameOver = GameObject.Find("GameOver");
        if (gameOver)
        {
            gameOver.SetActive(false);
        }
    }

    private void Update()
    {
        DrawStars();

        if (cleared == true)
        {
            Apocalypse();
        }
    }

    public void CreateStars()
    {
        float maxStarRemainder = gameManager.maxRating % 5;
        int starsToMake = (int)((gameManager.maxRating / 5) + maxStarRemainder);

        for (int i = 0; i < starsToMake; i++)
        {
            CreateEmptyStar();
        }
    }

    public void DrawStars()
    {
        for(int i = 0; i < stars.Count; i++)
        {
            int starStatusRemainder = (int)Mathf.Clamp(gameManager.reputation - (i*5), 0, 5);
            stars[i].SetStarImage((StarStatus)starStatusRemainder);
        }
    }

    public void CreateEmptyStar()
    {
        if (starPrefab == null) { return; }
        GameObject newStar = Instantiate(starPrefab);
        newStar.transform.SetParent(transform);

        starRating starComponent = newStar.GetComponent<starRating>();
        starComponent.SetStarImage(StarStatus.Empty);
        stars.Add(starComponent);
    }

    public void ClearStars()
    {
        for (int i=0; i< stars.Count; i++)
        {
            Destroy(transform.GetChild(stars.Count - i - 1));
        }
        stars = new List<starRating>();
    }


    private void Apocalypse()
    {
        generator.SetActive(false);
        p1.SetActive(false);
        p2.SetActive(false);
        cookMask.SetActive(false);
        GameObject[] brawls = (GameObject.FindGameObjectsWithTag("Brawl"));
        foreach (GameObject brawl in brawls)
            GameObject.Destroy(brawl);
        GameObject[] npcs = (GameObject.FindGameObjectsWithTag("NPC"));
        foreach (GameObject npc in npcs)
            GameObject.Destroy(npc);
        gameOver.SetActive(true);
    }
}
