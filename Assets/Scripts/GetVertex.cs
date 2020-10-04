﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVertex : MonoBehaviour
{
    public MeshFilter islandMesh;
    public GameObject prefab;
    public GameObject bigIsland;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Fader.instance.FadeOut(this);
        }
    }

    public void MoveToClosestPoint()
    {
        transform.position = GetNearbyVertex();
    }

    public Vector3 GetNearbyVertex()
    {
        // Get mesh
        Mesh mesh = islandMesh.mesh;
        // Set init values
        float minDistanceSqr = Mathf.Infinity;
        Vector3 nearestVertex = Vector3.zero;
        // Look for closest Vertex
        foreach (Vector3 vertex in mesh.vertices)
        {
            // Get Vertex w/ Rotation
            Vector3 diff = transform.position - (islandMesh.gameObject.transform.position + bigIsland.transform.rotation * vertex);
            float distSqr = diff.sqrMagnitude;

            // Return Closest
            if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                nearestVertex = vertex;
            }
        }

        Vector3 vertexPos = bigIsland.transform.rotation * nearestVertex + islandMesh.gameObject.transform.position;

        Vector3 newPos = vertexPos;
        Vector3 inVec = (bigIsland.transform.position + Vector3.up * 30.0f - vertexPos).normalized * 10.0f;

        newPos += inVec;


        //transform.position = nearestVertex;
        return (newPos + Vector3.up * 3.0f);
    }
}