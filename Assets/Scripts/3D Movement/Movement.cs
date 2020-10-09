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
    }
    #endregion Setup

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckBelow();
        }

        desiredRoll = new Vector3(body.transform.eulerAngles.x, body.transform.eulerAngles.y, myRoll);
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(desiredRoll), Time.deltaTime * rotationSpeed);

        desiredVec = new Vector3(myPitch, transform.eulerAngles.y + myTurn, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(desiredVec), Time.deltaTime * rotationSpeed);
    }

    [Header("Slowdown")]
    public GameObject front;
    public float dotProduct;

    private void FixedUpdate()
    {
        if (inRange)
        {
            Vector3 closestPoint = orbit.leashObject.GetComponent<MeshCollider>().ClosestPoint(front.transform.position);
            Vector3 closestPointOnBase = orbit.islandBase.ClosestPoint(front.transform.position);
            distance = Vector3.Distance(front.transform.position, closestPoint);

            Debug.DrawLine(front.transform.position, closestPoint, Color.red);
            float perc = Mathf.Clamp01(Vector3.Distance(front.transform.position, closestPoint) / maxDistance);
            Vector3 pointNorm = Vector3.Normalize(closestPoint - front.transform.position);
            dotProduct = 1 - Vector3.Dot(pointNorm, front.transform.forward);
            islandMod = Mathf.Clamp01((perc - (dotProduct * (1 - perc))) / dotProduct);


            Debug.DrawLine(front.transform.position, closestPointOnBase, Color.green);
            Vector3 dirToBase = closestPointOnBase - front.transform.position;
            if (dirToBase.y > 0)
            {
                float botPerc = Mathf.Clamp01(Vector3.Distance(front.transform.position, closestPointOnBase) / maxDistance);
                Vector3 botPointNorm = Vector3.Normalize(closestPointOnBase - front.transform.position);
                float botProduct = 1 - Vector3.Dot(botPointNorm, front.transform.forward);
                islandMod = Mathf.Clamp01((botPerc - (botProduct *(1 - botPerc))) / botProduct);
            }            
        }
        else
        {
            islandMod = 1.0f;
        }

        rb.MovePosition(transform.position + transform.forward * islandMod * currentSpeed * Time.deltaTime);
    }

    public float GetNearbyVertex()
    {
        MeshFilter meshFilter = orbit.leashObject.GetComponent<MeshFilter>();
        // Get mesh
        Mesh mesh = meshFilter.mesh;
        // Set init values
        float minDistanceSqr = Mathf.Infinity;
        Vector3 nearestVertex = Vector3.zero;
        // Look for closest Vertex
        foreach (Vector3 vertex in mesh.vertices)
        {
            // Get Vertex w/ Rotation
            Vector3 diff = transform.position - (meshFilter.gameObject.transform.position + meshFilter.transform.rotation * vertex);
            float distSqr = diff.sqrMagnitude;

            // Return Closest
            if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                nearestVertex = vertex;
            }
        }

        Vector3 vertexPos = meshFilter.transform.rotation * nearestVertex + meshFilter.gameObject.transform.position;

        Vector3 newPos = vertexPos;
        Vector3 inVec = (meshFilter.transform.position - vertexPos).normalized * 10.0f;

        newPos += inVec;


        //transform.position = nearestVertex;
        return (Vector3.Distance(newPos, transform.position));
    }

    public GameObject temp;
    RaycastHit hit;

    public void CheckBelow()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100.0f))
        {
            /*
             * Get the location of the hit.
             * This data can be modified and used to move your object.
             */
            temp.SetActive(true);
            temp.transform.position = hit.point;
            //Instantiate(temp, hit.point, Quaternion.identity);
            Debug.Log("Hit");
        }
        else
        {
            Debug.Log("No Hit");
        }
    }
}
