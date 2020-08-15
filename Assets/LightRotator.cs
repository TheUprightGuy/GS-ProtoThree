using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotator : MonoBehaviour
{
    public float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);
        transform.Rotate(Vector3.right * Time.deltaTime * rotSpeed);
    }
}
