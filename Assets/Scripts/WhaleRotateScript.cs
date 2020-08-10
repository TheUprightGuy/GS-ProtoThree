using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleRotateScript : MonoBehaviour
{
    public static WhaleRotateScript instance;
    private void Awake()
    {
        instance = this;
    }

    [Header("Setup Fields")]
    public GameObject lure;
    public RodRotationScript rod;
    public GameObject body;
    #region Local Variables
    public float rotationSpeed = 0.5f;
    // Cache Variables
    Vector3 lurePos;
    Vector3 myPos;
    Vector3 _direction;
    Quaternion _lookRotation;
    Vector3 forward = new Vector3(0, 0, 1);
    // Update Rot Countdown
    float countDown = 0.0f;
    public float currentSpeed = 0.0f;
    #endregion Local Variables

    public float debug;
    Vector3 roll = new Vector3(0, 0, 1);

    // Update is called once per frame
    void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, rod.speed, Time.deltaTime / 10);
        transform.Translate(forward * currentSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);

        if (countDown <= 0.0f)
        {
            UpdateRot();
            countDown = .5f;
        }
        countDown -= Time.deltaTime;
        

        if (Input.GetKey(KeyCode.A))
        {
            if (body.transform.localEulerAngles.z + Time.deltaTime * debug < 10 || body.transform.localEulerAngles.z + Time.deltaTime * debug >= 350)
            body.transform.eulerAngles += roll * Time.deltaTime * debug;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (body.transform.localEulerAngles.z - Time.deltaTime * debug > 350 || body.transform.localEulerAngles.z - Time.deltaTime * debug <= 10)
                body.transform.eulerAngles -= roll * Time.deltaTime * debug;
        }
    }

    public void UpdateRot()
    {
        lurePos = new Vector3(lure.transform.position.x, 0, lure.transform.position.z);
        myPos = new Vector3(transform.position.x, 0, transform.position.z);
        _direction = (lurePos - myPos).normalized;
        _lookRotation = Quaternion.LookRotation(_direction);
    }
}
