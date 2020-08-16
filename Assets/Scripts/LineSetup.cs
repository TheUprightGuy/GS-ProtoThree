using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LineSetup : MonoBehaviour
{
    public Transform LampModel;
    public Transform target;
    LineRenderer lr;
    CapsuleCollider cc;
    Vector3[] positions = new Vector3[2];
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        cc = GetComponent<CapsuleCollider>();

        positions[0] = target.position;
        positions[1] = transform.position;
        lr.SetPositions(positions);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = LampModel.position;
        if (Application.isPlaying)
        {
            positions[0] = target.position;
            positions[1] = transform.position;
            lr.SetPositions(positions);
            UpdateCollider();
        }
    }

    private void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
        cc = GetComponent<CapsuleCollider>();

        positions[0] = target.position;
        positions[1] = transform.position;
        lr.SetPositions(positions);
        UpdateCollider();
    }

    void UpdateCollider()
    {
        Vector3 dir = positions[0] - positions[1];
        float dist = Vector3.Distance(positions[1], positions[0]);
        transform.up = dir;
        cc.center = new Vector3(0.0f, dist / 2, 0.0f);
        cc.height = dist;
    }
}
