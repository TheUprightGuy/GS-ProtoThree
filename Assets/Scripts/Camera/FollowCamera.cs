using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Required Fields")]
    public GameObject whaleBody;
    public Transform originalPos;
    public Transform playerCamPos;
    public Quaternion originalRot;
    public GameObject target;

    [Header("Debug Fields")]
    public bool zoomIn = false;
    public bool zoomOut = false;
    public float speed = 5;
    Vector3 offset;

    float timer;
    public bool rotating = false;
    // Rotate Speeds
    private float xSpeed = 250.0f;
    private float ySpeed = 120.0f;
    // Y Angle Limits
    private int yMinLimit = -60;
    private int yMaxLimit = 60;
    // Zoom Speed
    private int zoomRate = 40;
    // Damping Rot & Xoom
    private float rotationDampening = 3.0f;
    private float zoomDampening = 5.0f;
    // Calculating Angles
    private float x = 0.0f;
    private float y = 0.0f;
    private float targetHeight;
    public float currentDistance;
    private float desiredDistance;
    private float correctedDistance;

    [Header("Follow Distance")]
    public float minDistance = .6f;
    public float maxDistance = 20;

    private float originalY;

    void Start()
    {
        offset = whaleBody.transform.position - transform.position;
        originalRot = transform.localRotation;
        targetHeight = whaleBody.GetComponent<Collider>().bounds.extents.y / 2;

        // Get Starting Properties
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        float distance = (minDistance + maxDistance) / 3.0f;
        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;
        target = whaleBody;

        currentDistance = Vector3.Distance(transform.position, target.transform.position);

        originalY = y;

    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            zoomIn = true;
            zoomOut = false;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < -0f)
        {
            zoomOut = true;
            zoomIn = false;
        }
    }

    void LateUpdate()
    {
        // Lerp back to pos
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }


        if (zoomIn)
        {
            target = playerCamPos.gameObject;
            transform.position = Vector3.Lerp(transform.position, playerCamPos.position, speed * Time.deltaTime / 2);
            currentDistance = Vector3.Distance(transform.position, target.transform.position);
            targetHeight = 0.0f;
            originalY = transform.rotation.eulerAngles.x;

            if (Vector3.Distance(transform.position, playerCamPos.position) < 0.5f)
            {
                zoomIn = false;
            }
        }
        else if (zoomOut)
        {
            target = whaleBody;
            transform.position = Vector3.Lerp(transform.position, originalPos.position, speed * Time.deltaTime / 2);
            currentDistance = Vector3.Distance(transform.position, target.transform.position);
            originalY = transform.rotation.eulerAngles.x;

            if (Vector3.Distance(transform.position, originalPos.position) < 0.5f)
            {
                zoomOut = false;
                target = whaleBody;
                currentDistance = Vector3.Distance(transform.position, target.transform.position);
                targetHeight = whaleBody.GetComponent<Collider>().bounds.extents.y / 2;
                originalY = transform.rotation.eulerAngles.x;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) { rotating = true; }
            if (Input.GetMouseButtonUp(0)) { rotating = false; }
            // Get Mouse Axis
            if (Input.GetMouseButton(0) && rotating)
            {

                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                timer = 1.5f;
            }
            // Ease on Movement
            else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || timer <= 0.0f)
            {
                float targetRotationAngle = target.transform.eulerAngles.y;
                float currentRotationAngle = transform.eulerAngles.y;
                x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);

                currentRotationAngle = transform.eulerAngles.x;
                if (currentRotationAngle > 90)
                {
                    currentRotationAngle -= 360.0f;
                }
                y = Mathf.LerpAngle(currentRotationAngle, originalY, rotationDampening * Time.deltaTime);
            }
            // Clamp Y Rotation
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            // Set Camera Rotation
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position;
            if (rotating)
            {
                position = target.transform.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -targetHeight, 0));
            }
            else
            {
                // Calc pos w/ new currentDistance
                position = Vector3.Lerp(transform.position, target.transform.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -targetHeight, 0)), speed * Time.deltaTime / 2);
            }

            // Set Position & Rotation
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        // Clamp Camera Angles
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return (Mathf.Clamp(angle, min, max));
    }
}
