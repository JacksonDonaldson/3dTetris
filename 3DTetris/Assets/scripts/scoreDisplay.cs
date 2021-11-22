using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreDisplay : MonoBehaviour
{
    public player p;
    public Text Score;


    // Update is called once per frame
    void Update()
    {
        Score.text = "SCORE: " + p.score;
    }
}
