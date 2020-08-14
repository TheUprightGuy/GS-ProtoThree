﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitScript : MonoBehaviour
{
    [Header("Debug Fields")]
    public GameObject leashObject;
    Vector3 axis = new Vector3(0,1,0);
    Vector3 objToIsland;
    Vector3 path;
    Quaternion lookRot;
    Rigidbody rb;
    [HideInInspector] public float initialSlerp = 0.0f;
    WhaleInfo whaleInfo;


    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        whaleInfo = CallbackHandler.instance.whaleInfo;
    }

    // Update is called once per frame
    void Update()
    {
        if (whaleInfo.leashed && leashObject)
        {
            objToIsland = leashObject.transform.position - transform.position;
            path = Vector3.Normalize(Vector3.Cross(objToIsland, axis));
            path = new Vector3(path.x, 0, path.z);

            if (initialSlerp > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(path), Time.deltaTime);
                initialSlerp -= Time.deltaTime;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(path), Time.deltaTime * 5);
            }

            /*transform.RotateAround(leashObject.transform.position, axis, rotationSpeed * Time.deltaTime);
            var desiredPosition = (transform.position - leashObject.transform.position).normalized * radius + leashObject.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);*/
            //transform.rotation = Quaternion.LookRotation(desiredPosition);
        }
    }

    private void FixedUpdate()
    {
        if (whaleInfo.leashed && leashObject)
        {
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
        }
        if (!leashObject)
        {
            whaleInfo.leashed = false;
        }
    }
}
