using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 WorldTopLeft = Vector3.zero;
    public Vector2 WorldBottomRight = Vector2.one;

    public Vector2 percentPos = Vector2.zero;

    public Transform trackingTransform;

    Vector3 centerPos = Vector3.zero;
    Bounds MapMeshBounds;
    void Start()
    {
        centerPos = transform.GetComponent<RectTransform>().localPosition;
        //MapMeshBounds = GetComponent<MeshFilter>().sharedMesh.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        float lengthX = WorldBottomRight.x - WorldTopLeft.x;
        float lengthY = WorldTopLeft.y - WorldBottomRight.y;

        float transformLengthX = trackingTransform.position.x - WorldTopLeft.x;
        float transformLengthY = trackingTransform.position.z - WorldBottomRight.y;

        float percentX = transformLengthX / lengthX;
        float percentY = transformLengthY / lengthY;

        percentPos.x = percentX;
        percentPos.y = percentY;

        Vector3 startPos = new Vector3(GetComponent<RectTransform>().rect.width / 2, (GetComponent<RectTransform>().rect.height / 2));
        Vector3 trackingPos = new Vector3(GetComponent<RectTransform>().rect.width * percentX, GetComponent<RectTransform>().rect.height * percentY); ;
        Vector3 pos = new Vector3(startPos.x - trackingPos.x, (startPos.y - trackingPos.y), 0.0f);
        GetComponent<RectTransform>().localPosition = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(WorldTopLeft.x, 0, WorldTopLeft.y), 10.0f);
        Gizmos.DrawSphere(new Vector3(WorldBottomRight.x, 0, WorldBottomRight.y), 10.0f);
    }
}
