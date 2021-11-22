using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scoreDisplay : MonoBehaviour
{
    public player p;
    private TextMeshProUGUI Score;

    void Start()
    {
        Score = gameObject.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
    }
    // Update is called once per frame
    void Update()
    {
        Score.text = "Score: " + p.score;
    }
}
