using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceMovement : MonoBehaviour
{
    Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = gameObject.GetComponent(typeof(Transform)) as Transform;
    }

    // Update is called once per frame
    void Update()
    {
        //cast down to see if there's anything there

        t.position = new Vector3(t.position.x, t.position.y - 1 * Time.deltaTime, t.position.z);
    }
}
