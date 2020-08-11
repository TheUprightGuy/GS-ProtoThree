using System.Collections;
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
        if (whaleInfo.leashed)
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
                transform.rotation = Quaternion.LookRotation(path);
            }
        }
    }

    private void FixedUpdate()
    {
        if (whaleInfo.leashed)
        {
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
        }
    }
}
