using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    #region Singleton
    public static Movement instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Whale Exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        rb = GetComponent<Rigidbody>();
    }
    #endregion Singleton 
    [Header("Setup Fields")]
    public GameObject body;
    private Rigidbody rb;
    public Animator animator;
    [Header("Upgrade Fields")]
    public float moveSpeed = 1;
    public float accelSpeed = 1;
    public float maxSpeed = 5.0f;
    [HideInInspector] public float turnSpeed = 40;
    [HideInInspector] public float liftSpeed = 20;
    [HideInInspector] public float rollSpeed = 20;

    #region Local Variables
    [Header("Local Variables")]
    public float rotationSpeed = 0.2f;
    // Cache Variables
    Vector3 lurePos;
    Vector3 myPos;
    Vector3 _direction;
    Quaternion _lookRotation;
    //[HideInInspector]
    public float currentSpeed = 0.0f;
    Vector3 desiredVec;
    Vector3 desiredRoll;
    public float myRoll = 0.0f;
    public float myTurn = 0.0f;
    public float myPitch = 0.0f;
    #endregion Local Variables

    #region Setup
    private void Start()
    {
        desiredVec = body.transform.eulerAngles;
    }
    #endregion Setup

    // Update is called once per frame
    void Update()
    {
        float movement = currentSpeed / 2;

        float f = body.transform.rotation.eulerAngles.z;
        f = (f > 180) ? f - 360 : f;
        animator.SetFloat("Turning", f / 10.0f);
        animator.SetFloat("Movement", movement);

        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * accelSpeed);

        float slowSpeedTurnBonus = maxSpeed / moveSpeed;

        if (Input.GetKey(KeyCode.D))
        {
            if (myTurn + Time.deltaTime * turnSpeed * slowSpeedTurnBonus < 30)
            {
                myTurn += Time.deltaTime * turnSpeed * slowSpeedTurnBonus;
            }
            if (myRoll - Time.deltaTime * rollSpeed > -10)
            {
                myRoll -= Time.deltaTime * rollSpeed;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (myTurn - Time.deltaTime * turnSpeed * slowSpeedTurnBonus > -30)
            {
                myTurn -= Time.deltaTime * turnSpeed * slowSpeedTurnBonus;
            }
            if (myRoll + Time.deltaTime * rollSpeed < 10)
            {
                myRoll += Time.deltaTime * rollSpeed;
            }
        }

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

        if (Input.GetKey(KeyCode.Space))
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

        desiredRoll = new Vector3(body.transform.eulerAngles.x, body.transform.eulerAngles.y, myRoll);
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(desiredRoll), Time.deltaTime * rotationSpeed);

        desiredVec = new Vector3(myPitch, myTurn, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(desiredVec), Time.deltaTime * rotationSpeed);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);     
    }
}
