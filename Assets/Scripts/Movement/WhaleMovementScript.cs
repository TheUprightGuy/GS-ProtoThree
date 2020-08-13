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
    public WhaleInfo whaleInfo;
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
        whaleInfo.whale = this.gameObject;
        desiredRoll = body.transform.eulerAngles;
    }
    #endregion Setup

    public Vector3 desiredRoll;
    public float myRoll = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(EventHandler.instance.gameState.gamePaused) return;
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
                if (myRoll + Time.deltaTime * turnSpeed < 10)
                {
                    myRoll += Time.deltaTime * turnSpeed;
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (myRoll - Time.deltaTime * turnSpeed > -10)
                {
                    myRoll -= Time.deltaTime * turnSpeed;
                }
            }
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed));

            desiredRoll = new Vector3(body.transform.eulerAngles.x, body.transform.eulerAngles.y, myRoll);
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(desiredRoll), Time.deltaTime * rotationSpeed);
        }
        else
        {
            if (body.transform.localEulerAngles.z - Time.deltaTime * turnSpeed > 350 || body.transform.localEulerAngles.z - Time.deltaTime * turnSpeed <= 10)
                body.transform.eulerAngles -= roll * Time.deltaTime * turnSpeed;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (inRange)
            {
                whaleInfo.ToggleLeashed(!whaleInfo.leashed);
                orbit.initialSlerp = 2.1f;
            }
            else
            {
                whaleInfo.ToggleLeashed(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!whaleInfo.leashed)
        {
            rb.MovePosition(transform.position + transform.forward * currentSpeed * whaleInfo.hungerModifier * Time.deltaTime);
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

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Birds"))
        {
            whaleInfo.FeedWhale();
        }
    }
}
