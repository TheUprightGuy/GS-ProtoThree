using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMovementScript : MonoBehaviour
{
    #region Singleton
    public static WhaleMovementScript instance;
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
        orbit = GetComponent<OrbitScript>();
    }
    #endregion Singleton 
    [Header("Setup Fields")]
    public GameObject lure;
    public RodRotationScript rod;
    public GameObject body;
    private Rigidbody rb;
    [HideInInspector] public OrbitScript orbit;
    WhaleInfo whaleInfo;
    [Header("Upgrade Fields")]
    public float turnSpeed = 20;
    public float moveSpeed = 1;
    public float accelSpeed = 1;
    [Header("Debug Fields")]
    public bool inRange = false;
    
    [Header("Local Variables")]
    #region Local Variables
    public float rotationSpeed = 0.5f;
    // Cache Variables
    Vector3 lurePos;
    Vector3 myPos;
    Vector3 _direction;
    Quaternion _lookRotation;
    Vector3 roll = new Vector3(0, 0, 1);
    // Update Rot Countdown
    float countDown = 0.0f;
    public float currentSpeed = 0.0f;
    #endregion Local Variables
    #region Setup
    private void Start()
    {
        whaleInfo = CallbackHandler.instance.whaleInfo;    
    }
    #endregion Setup

    // Update is called once per frame
    void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, rod.speed * moveSpeed, Time.deltaTime * accelSpeed);

        if (countDown <= 0.0f)
        {
            UpdateRot();
            countDown = .5f;
        }
        countDown -= Time.deltaTime;

        if (!whaleInfo.leashed)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (body.transform.localEulerAngles.z + Time.deltaTime * turnSpeed < 10 || body.transform.localEulerAngles.z + Time.deltaTime * turnSpeed >= 350)
                    body.transform.eulerAngles += roll * Time.deltaTime * turnSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (body.transform.localEulerAngles.z - Time.deltaTime * turnSpeed > 350 || body.transform.localEulerAngles.z - Time.deltaTime * turnSpeed <= 10)
                    body.transform.eulerAngles -= roll * Time.deltaTime * turnSpeed;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            whaleInfo.ToggleLeashed(!whaleInfo.leashed);
            orbit.initialSlerp = 1.7f;
        }
    }

    private void FixedUpdate()
    {
        if (!whaleInfo.leashed)
        {
            rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);
        }       
    }

    public void UpdateRot()
    {
        if (!whaleInfo.leashed)
        {
            lurePos = new Vector3(lure.transform.position.x, 0, lure.transform.position.z);
            myPos = new Vector3(transform.position.x, 0, transform.position.z);
            _direction = (lurePos - myPos).normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
        }
    }
}
