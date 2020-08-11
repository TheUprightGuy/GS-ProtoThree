using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Transform currentCam;

    public Transform playerCam;
    public Transform whaleCam;
    public Transform whale;

    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 13f;

    float x = 0.0f;
    float y = 0.0f;

    bool zoomIn = false;
    bool zoomOut = false;

    float timer = 0.0f;
    float lerpTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        distance = distanceMax;

        currentCam = whaleCam;
    }

    void LateUpdate()
    {
        timer -= Time.deltaTime;
        lerpTimer -= Time.deltaTime;

        if (zoomIn)
        {
            currentCam = playerCam;
            transform.position = Vector3.Lerp(transform.position, playerCam.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, playerCam.rotation, Time.deltaTime);
            if (Vector3.Distance(transform.position, playerCam.position) < 0.5f)
            {
                zoomIn = false;
                target = playerCam;
                lerpTimer = 1.0f;
            }
            distance = distanceMin;
        }
        else if (zoomOut)
        {
            currentCam = whaleCam;
            transform.position = Vector3.Lerp(transform.position, whaleCam.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, whaleCam.rotation, Time.deltaTime);
            if (Vector3.Distance(transform.position, whaleCam.position) < 0.5f)
            {
                zoomOut = false;
                target = whale;
                lerpTimer = 1.0f;
            }
            distance = distanceMax;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                timer = 1.5f;

                transform.RotateAround(target.position,
                                                Vector3.up,
                                                Input.GetAxis("Mouse X") * xSpeed);

                transform.RotateAround(target.position,
                                                transform.right,
                                                -Input.GetAxis("Mouse Y") * ySpeed);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                zoomIn = true;
                zoomOut = false;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backward
            {
                zoomOut = true;
                zoomIn = false;
            }

            if (timer <= 0.0f)
            {

                float targetRotationAngle = currentCam.eulerAngles.y;
                float currentRotationAngle = transform.eulerAngles.y;
                x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, 5 * Time.deltaTime);
                targetRotationAngle = currentCam.eulerAngles.x;
                currentRotationAngle = transform.eulerAngles.x;
                if (currentRotationAngle > 90)
                {
                    currentRotationAngle -= 360.0f;
                }

                y = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, 5 * Time.deltaTime);
                // Clamp Y Rotation
                y = ClampAngle(y, yMinLimit, yMaxLimit);
                // Set Camera Rotation
                Quaternion rotation = Quaternion.Euler(y, x, 0);

                // Calc pos w/ new currentDistance
                Vector3 position = target.position - (rotation * Vector3.forward * distance);
                Vector3 adjust = Vector3.Lerp(transform.position, position, 5 * Time.deltaTime);
                // Set Position & Rotation
                transform.rotation = rotation;
                if (lerpTimer > 0)
                {
                    transform.position = adjust;
                }
                else
                {
                    transform.position = position;
                }
            }
        }

    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
