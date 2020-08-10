using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleRotateScript : MonoBehaviour
{
    [Header("Setup Fields")]
    public GameObject lure;
    public RodRotationScript rod;
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
    #endregion Local Variables



    // Update is called once per frame
    void Update()
    {
        transform.Translate(forward * rod.speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);

        if (countDown <= 0.0f)
        {
            UpdateRot();
            countDown = .5f;
        }
        countDown -= Time.deltaTime;
    }

    public void UpdateRot()
    {
        lurePos = new Vector3(lure.transform.position.x, 0, lure.transform.position.z);
        myPos = new Vector3(transform.position.x, 0, transform.position.z);
        _direction = (lurePos - myPos).normalized;
        _lookRotation = Quaternion.LookRotation(_direction);
    }
}
