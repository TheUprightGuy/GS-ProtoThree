using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Required Fields")]
    public GameObject whaleBody;
    public Transform layerCamPos;

    [Header("Debug Fields")]
    public bool zoomIn = false;
    public float speed = 5;
    Vector3 offset;

    void Start()
    {
        offset = whaleBody.transform.position - transform.position;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            zoomIn = true;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < -0f)
        {
            zoomIn = false;
        }
    }

    void LateUpdate()
    {     
        if (zoomIn)
        {
            transform.position = Vector3.Lerp(transform.position, layerCamPos.position, speed * Time.deltaTime / 2);
            transform.rotation = Quaternion.Slerp(transform.rotation, layerCamPos.rotation, speed * Time.deltaTime);
        }
        else
        {
            // Look
            var newRotation = Quaternion.LookRotation(whaleBody.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed * Time.deltaTime);

            // Move
            Vector3 newPosition = whaleBody.transform.position - whaleBody.transform.forward * offset.z - whaleBody.transform.up * offset.y;
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed / 2);
        }
    }
}
