using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodRotationScript : MonoBehaviour
{
    [Header("Rotation Limits")]
    public float rotLimit = 0.7f;
    public float speedLimit = 0.4f;
    [Header("Setup Fields")]
    public GameObject lure;
    [HideInInspector] public float rotSpeed = 1.0f;
    // Local Vars
    float zRot = 0.0f;
    float xRot = 0.0f;
    public float speed = 0.0f;
    public float debug = 0.0f;

    // Update is called once per frame
    void Update()
    {
        speed = -xRot / speedLimit + 1.5f;

        if (Input.GetKey(KeyCode.A))
        {
            if (zRot - Time.deltaTime * rotSpeed < -rotLimit)
            {
                zRot = -rotLimit;
            }
            else
            {
                zRot -= Time.deltaTime * rotSpeed;
                transform.localEulerAngles -= new Vector3(0, Time.deltaTime * debug, 0);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (zRot + Time.deltaTime * rotSpeed > rotLimit)
            {
                zRot = rotLimit;
            }
            else
            {
                zRot += Time.deltaTime * rotSpeed;
                transform.localEulerAngles += new Vector3(0, Time.deltaTime * debug, 0);
            }
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            if (xRot - Time.deltaTime * rotSpeed < -speedLimit)
            {
                xRot = -speedLimit;
            }
            else
            {
                xRot -= Time.deltaTime * rotSpeed;
                transform.localEulerAngles -= new Vector3(Time.deltaTime * debug, 0, 0);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (xRot + Time.deltaTime * rotSpeed > speedLimit)
            {
                xRot = speedLimit;
            }
            else
            {
                xRot += Time.deltaTime * rotSpeed;
                transform.localEulerAngles += new Vector3(Time.deltaTime * debug, 0, 0);
            }
        }
    }
}
