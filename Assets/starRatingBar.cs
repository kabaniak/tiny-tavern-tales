using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starRatingBar : MonoBehaviour
{
    public GameObject starPrefab;
    public GameManager gameManager;
    List<starRating> stars = new List<starRating>();

    public void Start()
    {
        ClearStars();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        CreateStars();
        DrawStars();
    }

    private void Update()
    {
        DrawStars();
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
}
