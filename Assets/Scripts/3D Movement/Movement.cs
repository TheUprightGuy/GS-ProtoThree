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
    [Header("Tracking")]
    public OrbitScript orbit;
    public bool inRange;
    public float maxDistance;
    public float angle;
    public float islandMod = 0.0f;
    public float distance;
    public bool orbiting;

    #region Local Variables
    WhaleInfo whaleInfo;
    [Header("Local Variables")]
    public float rotationSpeed = 0.2f;
    // Cache Variables
    //[HideInInspector]
    public float currentSpeed = 0.0f;
    Vector3 desiredVec;
    Vector3 desiredRoll;
    [HideInInspector] public float myRoll = 0.0f;
    [HideInInspector] public float myTurn = 0.0f;
    [HideInInspector] public float myPitch = 0.0f;
    [HideInInspector] public float turnSpeed = 40;
    [HideInInspector] public float liftSpeed = 20;
    [HideInInspector] public float rollSpeed = 20;
    #endregion Local Variables
    #region Setup
    private void Start()
    {
        desiredVec = body.transform.eulerAngles;
        temp.SetActive(false);
        whaleInfo = CallbackHandler.instance.whaleInfo;

        CallbackHandler.instance.pickUpMC += PickUpMC;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.pickUpMC -= PickUpMC;
    }
    #endregion Setup

    // Update is called once per frame
    void Update()
    {
        if (orbiting)
        {
            animator.SetFloat("Movement", currentSpeed * islandMod / 2);
            currentSpeed = Mathf.Lerp(currentSpeed, 1.0f, Time.deltaTime);
            return;
        }

        if (homing)
        {
            animator.SetFloat("Movement", currentSpeed * islandMod / 2);
            currentSpeed = Mathf.Lerp(currentSpeed, 3.0f, Time.deltaTime);
            return;
        }

        float movement = currentSpeed * islandMod / 2;

        float f = body.transform.rotation.eulerAngles.z;
        f = (f > 180) ? f - 360 : f;
        animator.SetFloat("Turning", f / 10.0f);
        animator.SetFloat("Movement", movement);

        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * accelSpeed);   

        float slowSpeedTurnBonus = (maxSpeed / currentSpeed);

        if (Input.GetKey(KeyCode.D))
        {

            if (myTurn + Time.deltaTime * turnSpeed * slowSpeedTurnBonus < 40)
            {
                myTurn += Time.deltaTime * turnSpeed * slowSpeedTurnBonus;
            }
           
            if (myRoll - Time.deltaTime * rollSpeed > -10)
            {
                myRoll -= Time.deltaTime * rollSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (myTurn - Time.deltaTime * turnSpeed * slowSpeedTurnBonus > -40)
            {
                myTurn -= Time.deltaTime * turnSpeed * slowSpeedTurnBonus;
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

        if (orbit.leashObject && CheckBelow() != Vector3.zero)
        {
            CallbackHandler.instance.LandingTooltip(true);
        }
        else
        {
            CallbackHandler.instance.LandingTooltip(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CheckBelow() != Vector3.zero)
            {
                //Orbit(true);
                orbit.leashObject.GetComponent<MeshCollider>().convex = false;
                Fader.instance.FadeOut(this);
                orbiting = true;
                CallbackHandler.instance.LandingTooltip(false);
                // Called by Animator
                // MoveCharacter();
            }
        }
        desiredRoll = new Vector3(body.transform.eulerAngles.x, body.transform.eulerAngles.y, myRoll);
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(desiredRoll), Time.deltaTime * rotationSpeed);

        desiredVec = new Vector3(myPitch, transform.eulerAngles.y + myTurn, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(desiredVec), Time.deltaTime * rotationSpeed);
    }

    public GameObject player;
    public GameObject rider;
    public Cinemachine.CinemachineFreeLook followCam;
    public FollowCamera followCamera;

    public void MoveCharacter()
    {
        followCam.gameObject.SetActive(true);
        rider.SetActive(false);
        player.SetActive(true);
        player.transform.parent = null;
        player.transform.position = CheckBelow();
        followCamera.enabled = false;
        //player.GetComponent<Rigidbody>().useGravity = true;
    }

    public void PickUpMC()
    {
        followCam.gameObject.SetActive(false);
        rider.SetActive(true);
        player.SetActive(false);
        orbiting = false;
        homing = false;
        whaleInfo.ToggleLeashed(false);
        exit = true;
        orbit.leashObject.GetComponent<MeshCollider>().convex = true;
        followCamera.enabled = true;
    }

    [Header("Slowdown")]
    public GameObject front;
    public float dotProduct;
    public bool homing;
    public bool exit;

    private void FixedUpdate()
    {
        if (orbiting || homing)
        {
            rb.MovePosition(transform.position + transform.forward * islandMod * currentSpeed * Time.deltaTime);
            return;
        }

        if (exit)
        {
            rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);
            return;
        }

        if (inRange)
        {
            distance = Mathf.Infinity;

            if (orbit.leashObject)
            {
                Vector3 closestPoint = orbit.leashObject.GetComponent<MeshCollider>().ClosestPoint(front.transform.position);
                distance = Vector3.Distance(closestPoint, front.transform.position);

                Debug.DrawLine(front.transform.position, closestPoint, Color.red);

                // New Attempt
                if (distance < 30.0f)
                {
                    float perc = Mathf.Clamp01(distance / 30.0f);

                    Vector3 pointNorm = Vector3.Normalize(closestPoint - front.transform.position);
                    dotProduct = 1 - Vector3.Dot(front.transform.forward, pointNorm);
                    islandMod = Mathf.Clamp01(perc / dotProduct);
                }
            }
        }
        else
        {
            islandMod = 1.0f;
        }

        rb.MovePosition(transform.position + transform.forward * islandMod * currentSpeed * Time.deltaTime);
    }

    public GameObject temp;
    RaycastHit hit;

    public Vector3 CheckBelow()
    {
        Vector3 closestPoint = orbit.leashObject.GetComponent<MeshCollider>().ClosestPoint(front.transform.position);
        float checkDistance = Vector3.Distance(front.transform.position, closestPoint);

        Vector3 dir = closestPoint - front.transform.position;

        if (Physics.Raycast(front.transform.position, Vector3.down, out hit, 100.0f))
        {
            /*
             * Get the location of the hit.
             * This data can be modified and used to move your object.
             */
            //temp.SetActive(true);
            //temp.transform.position = hit.point;
            //Instantiate(temp, hit.point, Quaternion.identity);
            Debug.Log("Hit");
            return hit.point;
        }
        else if (checkDistance < 12.0f && dir.y < 0)
        {
            //temp.SetActive(true);
            //temp.transform.position = hit.point;
            // Need to add a slight push inwards to the island
            Debug.Log("Side Hit");
            return closestPoint;
        }
        else
        {
            Debug.Log("No Hit");
            return Vector3.zero;
        }
    }

    public void Orbit(bool _toggle)
    {
        if (inRange)
        {
            whaleInfo.ToggleLeashed(_toggle);
            if (whaleInfo.leashed)
            {
                orbit.SetOrbitDirection();
            }
        }
        else
        {
            whaleInfo.ToggleLeashed(false);
        }
    }
}
