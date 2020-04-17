using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    int points = 0;
    [SerializeField] Text pointsCounter;

    internal void addPoints()
    {
        points += 10;
        pointsCounter.text = points.ToString();
    }
}
