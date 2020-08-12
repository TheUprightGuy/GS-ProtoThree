using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTSCamera : MonoBehaviour
{
    #region Singleton
    public static OTSCamera instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple Cameras Exist!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton

    // Player & Attributes
    private Transform target;
    private float targetHeight;
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
    // Calculating Distance
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;

    [Header("Follow Distance")]
    public float minDistance = .6f;
    public float maxDistance = 20;
    [Header("Ease Timer")]
    public float timer = 0.0f;

    public bool rotating;

    void Start()
    {
        // Singletons
        if (!WhaleMovementScript.instance)
        {
            Debug.LogError("Player Instance is Missing from Camera!");
        }
        else
        {
            target = WhaleMovementScript.instance.gameObject.transform;
            targetHeight = target.GetComponent<Collider>().bounds.extents.y / 2;
        }
        // Get Starting Properties
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        float distance = (minDistance + maxDistance) / 3.0f;
        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;
    }

    void LateUpdate()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            rotating = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            rotating = false;
        }
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
            float targetRotationAngle = target.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
        }
        // Clamp Y Rotation
        y = ClampAngle(y, yMinLimit, yMaxLimit);
        // Set Camera Rotation
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // Calc pos w/ new currentDistance
        Vector3 position = target.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -targetHeight, 0));

        // Set Position & Rotation
        transform.rotation = rotation;
        transform.position = position;
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
