using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class starRating : MonoBehaviour
{
    public Sprite fullStar, fourFStar, threeFStar, twoFStar, oneFStar, emptyStar;
    Image starImage;

    private void Awake()
    {
        starImage = GetComponent<Image>();
    }

    public void SetStarImage(StarStatus status)
    {
        switch (status)
        {
            case StarStatus.Empty:
                starImage.sprite = emptyStar;
                break;
        }
        switch (status)
        {
            case StarStatus.OneFive:
                starImage.sprite = oneFStar;
                break;
        }
        switch (status)
        {
            case StarStatus.TwoFive:
                starImage.sprite = twoFStar;
                break;
        }
        switch (status)
        {
            case StarStatus.ThreeFive:
                starImage.sprite = threeFStar;
                break;
        }
        switch (status)
        {
            case StarStatus.FourFive:
                starImage.sprite = fourFStar;
                break;
        }
        switch (status)
        {
            case StarStatus.Full:
                starImage.sprite = fullStar;
                break;
        }
    }
}

public enum StarStatus
{
    Empty = 0,
    OneFive = 1,
    TwoFive = 2,
    ThreeFive = 3,
    FourFive = 4,
    Full = 5
}