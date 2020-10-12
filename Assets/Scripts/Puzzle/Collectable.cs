using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 1f, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collectable found!");
        Destroy(gameObject);
    }
}
