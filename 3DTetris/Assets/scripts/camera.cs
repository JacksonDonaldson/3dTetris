using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 lookLocation;
    public float lookSens;
    public GameObject floor;

    public Vector3 up;
    public Vector3 down;
    public Vector3 left;
    public Vector3 right;

    public string roll, pitch, yaw;
    public bool pitchpol, rollpol, yawpol;

    private int direction = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //print(direction);

        Vector3 axis= Vector3.zero;
        switch (direction){
            case 0:
                up = Vector3.forward;
                down = Vector3.back;
                left = Vector3.left;
                right = Vector3.right;

                pitch = "x";
                yaw = "y";
                roll = "z";

                pitchpol = false;
                yawpol = false;
                rollpol = false;

                axis = Vector3.right;
                break;
            case 1:

                pitch = "z";
                roll = "x";
                yaw = "y";
                pitchpol = false;
                yawpol = false;
                rollpol = true;

                right = Vector3.forward;
                left = Vector3.back;
                up = Vector3.left;
                down = Vector3.right;



                axis = Vector3.forward;
                break;
            case 2:

                pitch = "x";
                roll = "z";
                yaw = "y";

                pitchpol = true;
                yawpol = false;
                rollpol = true;

                down = Vector3.forward;
                up = Vector3.back;
                right = Vector3.left;
                left = Vector3.right;
                axis = Vector3.left;
                break;

            case 3:

                pitchpol = true;
                yawpol = false;
                rollpol = false;

                pitch = "z";
                roll = "x";
                yaw = "y";

                left = Vector3.forward;
                right = Vector3.back;
                down = Vector3.left;
                up = Vector3.right;

                axis = Vector3.back;
                break;
        }


        transform.RotateAround(lookLocation, axis, lookSens * Input.GetAxis("CameraVertical") * Time.deltaTime);


        if (transform.eulerAngles.x > 60)
        {
            transform.RotateAround(lookLocation, axis, -lookSens * Input.GetAxis("CameraVertical") * Time.deltaTime);
        }

        transform.LookAt(lookLocation);


        if (Input.GetButtonDown("CameraRight"))
        {
            //floor.transform.eulerAngles = new Vector3(floor.transform.eulerAngles.x, floor.transform.eulerAngles.y + 90, floor.transform.eulerAngles.z);
            transform.RotateAround(lookLocation, Vector3.up, -90);
            direction = (direction + 1) % 4; 
        }
        if (Input.GetButtonDown("CameraLeft"))
        {
            //floor.transform.eulerAngles = new Vector3(floor.transform.eulerAngles.x, floor.transform.eulerAngles.y - 90, floor.transform.eulerAngles.z);
            //because c# mod is stupid
            direction = (direction +3) % 4;
            transform.RotateAround(lookLocation, Vector3.up, 90);
        }
    }
}
