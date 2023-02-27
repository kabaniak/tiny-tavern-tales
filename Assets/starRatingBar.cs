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
        DrawStars();
    }

    public void DrawStars()
    {
        ClearStars();

        float maxStarRemainder = gameManager.maxRating % 5;
        int starsToMake = (int)((gameManager.maxRating / 5) + maxStarRemainder);

        for(int i = 0; i < starsToMake; i++)
        {
            CreateEmptyStar();
        }

        for(int i = 0; i < stars.Count; i++)
        {
            int starStatusRemainder = (int)Mathf.Clamp(gameManager.reputation - (i*5), 0, 5);
            stars[i].SetStarImage((StarStatus)starStatusRemainder);
        }
    }

    public void CreateEmptyStar()
    {
        GameObject newStar = Instantiate(starPrefab);
        newStar.transform.SetParent(transform);

        starRating starComponent = newStar.GetComponent<starRating>();
        starComponent.SetStarImage(StarStatus.Empty);
        stars.Add(starComponent);
    }

    public void ClearStars()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        stars = new List<starRating>();
    }
}
