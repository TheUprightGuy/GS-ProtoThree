using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(ObjDistribuition))]
public class ObjDistributionEditor : Editor
{
    private Bounds bounds = new Bounds();
    void OnSceneGUI()
    {
        ObjDistribuition myObj = target as ObjDistribuition;

        Handles.DrawWireCube(myObj.transform.position, myObj.transform.localScale);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ObjDistribuition t = target as ObjDistribuition;
        if (GUILayout.Button("Reload"))
        {
            t.PlaceObjMesh();
        }


        
    }
}
