using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nextDisplay : MonoBehaviour
{
    public player p;

    public Sprite orange;
    public Sprite purple;
    public Sprite cyan;
    public Sprite yellow;
    public Sprite red;
    public Sprite green;

    private Image i;

    void Start()
    {
        i = gameObject.GetComponent(typeof(Image)) as Image;
    }
    // Update is called once per frame
    void Update()
    {
        switch (p.nextTet){
            case 0:
                i.sprite = green;
                break;
            case 1:
                i.sprite = red;
                break;
            case 2:
                i.sprite = cyan;
                break;
            case 3:
                i.sprite = orange;
                break;
            case 4:
                i.sprite = purple;
                break;
            case 5:
                i.sprite = yellow;
                break;
        }
    }
}
