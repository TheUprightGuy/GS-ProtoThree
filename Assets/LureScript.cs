using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureScript : MonoBehaviour
{
    #region Singleton
    public static LureScript instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Lure Exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton
    [Header("Current Position")]
    public float currentSide;
    public float currentForward;
    public float speed;
    [Header("Limits")]
    public float sideLimit = 5.0f;
    public float backLimit = -1.25f;
    public float forwardLimit = 3.0f;
    [Header("Speeds")]
    public float turnSpeed = 10.0f;
    public float forwardSpeed = 10.0f;


    // Update is called once per frame
    void Update()
    {
        if (EventHandler.instance.gameState.gamePaused) return;

        speed = (currentForward + 2.0f) / 2.0f;

        if (Input.GetKey(KeyCode.A))
        {
            if (currentSide - Time.deltaTime * turnSpeed > -sideLimit)
            {
                currentSide -= Time.deltaTime * turnSpeed;
                transform.position -= transform.right * Time.deltaTime * turnSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (currentSide + Time.deltaTime * turnSpeed < sideLimit)
            {
                currentSide += Time.deltaTime * turnSpeed;
                transform.position += transform.right * Time.deltaTime * turnSpeed;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (currentForward + Time.deltaTime * forwardSpeed < forwardLimit)
            {
                currentForward += Time.deltaTime * forwardSpeed;
                transform.position += transform.forward * Time.deltaTime * forwardSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (currentForward + Time.deltaTime * forwardSpeed > backLimit)
            {
                currentForward -= Time.deltaTime * forwardSpeed;
                transform.position -= transform.forward * Time.deltaTime * forwardSpeed;
            }
        }
    }
}
