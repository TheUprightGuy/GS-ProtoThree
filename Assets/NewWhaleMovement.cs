﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWhaleMovement : MonoBehaviour
{
    [Header("Setup Fields")]
    public GameObject body;
    private Rigidbody rb;
    public Animator animator;
    [Header("Movement")]
    public float currentSpeed = 0.0f;
    public float moveSpeed = 1;
    public float accelSpeed = 1;
    public float maxSpeed = 5.0f;
    public float minimumDistance = 15.0f;

    #region Local Variables
    [HideInInspector] public bool tooClose;
    [HideInInspector] public bool control = true;
    float buckTimer = 1.0f;
    float distance;
    float rotationSpeed = 0.2f;
    Vector3 desiredVec;
    Vector3 desiredRoll;
    float myRoll = 0.0f;
    float myTurn = 0.0f;
    float myPitch = 0.0f;
    float turnSpeed = 40;
    float liftSpeed = 20;
    float rollSpeed = 20;
    #endregion Local Variables

    NewOrbitScript orbit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        orbit = GetComponent<NewOrbitScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        orbit.enabled = false;
        desiredVec = body.transform.eulerAngles;
    }

    public void TurningControl()
    {
        // Yaw
        if (Input.GetKey(KeyCode.D))
        {

            if (myTurn + Time.deltaTime * turnSpeed < 40)
            {
                myTurn += Time.deltaTime * turnSpeed;
            }

            if (myRoll - Time.deltaTime * rollSpeed > -10)
            {
                myRoll -= Time.deltaTime * rollSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (myTurn - Time.deltaTime * turnSpeed > -40)
            {
                myTurn -= Time.deltaTime * turnSpeed;
            }

            if (myRoll + Time.deltaTime * rollSpeed < 10)
            {
                myRoll += Time.deltaTime * rollSpeed;
            }
        }
        else
        {
            myTurn = Mathf.Lerp(myTurn, 0, Time.deltaTime * turnSpeed);
            myRoll = Mathf.Lerp(myRoll, 0, Time.deltaTime * rollSpeed * 5);
        }
        // Pitch
        if (Input.GetKey(KeyCode.W))
        {
            if (myPitch + Time.deltaTime * liftSpeed < 30)
            {
                myPitch += Time.deltaTime * liftSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (myPitch - Time.deltaTime * liftSpeed > -30)
            {
                myPitch -= Time.deltaTime * liftSpeed;
            }
        }
        else
        {
            myPitch = Mathf.Lerp(myPitch, 0.0f, Time.deltaTime);
        }
    }
    public void SpeedControl()
    {
        // Move
        if (Input.GetKey(InputHandler.instance.move))
        {
            if (moveSpeed < maxSpeed)
            {
                moveSpeed += accelSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (moveSpeed > maxSpeed / 2)
            {
                moveSpeed -= accelSpeed * Time.deltaTime;
            }
            else if (moveSpeed > maxSpeed / 3)
            {
                moveSpeed -= accelSpeed * (Time.deltaTime / 2);
            }
            else if (moveSpeed > 1.0f)
            {
                moveSpeed -= accelSpeed * Time.deltaTime * 0.1f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float movement = currentSpeed / 2;
        float f = body.transform.rotation.eulerAngles.z;
        f = (f > 180) ? f - 360 : f;
        animator.SetFloat("Turning", f / 10.0f);
        animator.SetFloat("Movement", movement);
        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * accelSpeed);

        if (control)
        {
            TurningControl();
            SpeedControl();
        }
        GetDistance();
    }

    public void GetDistance()
    {
        Debug.DrawRay(transform.position, transform.forward * Mathf.Infinity, Color.green);
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, GetComponent<CapsuleCollider>().height / 2, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Island"))
            {
                distance = hit.distance;
                if (distance < minimumDistance)
                {
                    orbit.SetupOrbit(hit.transform.gameObject.GetComponent<NewIslandScript>().heightRef);
                    //orbit.orbit = hit.transform.gameObject.GetComponent<NewIslandScript>().heightRef;
                    tooClose = true;
                    control = false;
                    Debug.Log("Crashing!");
                }
                Debug.Log("Island in Front");
            }
        }
    }

    private void FixedUpdate()
    {
        desiredRoll = new Vector3(body.transform.eulerAngles.x, body.transform.eulerAngles.y, myRoll);
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(desiredRoll), Time.deltaTime * rotationSpeed);
        // Rot
        desiredVec = new Vector3(myPitch, transform.eulerAngles.y + myTurn, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(desiredVec), Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0)), Time.deltaTime * 10.0f);

        if (tooClose)
        {
            rb.MovePosition(transform.position - transform.forward * currentSpeed * Time.deltaTime);
            buckTimer -= Time.deltaTime;
            if (buckTimer <= 0)
            {
                tooClose = false;
                orbit.enabled = true;
            }
        }
        else
        {
            rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);
        }
    }
}