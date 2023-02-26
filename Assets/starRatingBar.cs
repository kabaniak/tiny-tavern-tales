using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starRatingBar : MonoBehaviour
{
    public GameObject starPrefab;
    public GameManager gameManager;
    List<starRating> stars = new List<starRating>();

    public void DrawStars()
    {
        ClearStars();

        float maxStarRemainder = gameManager.maxRating % 5;
        int starsToMake = (int)((gameManager.maxRating / 2) + maxStarRemainder);

        for(int i = 0; i < starsToMake; i++)
        {
            CreateEmptyStar();
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
