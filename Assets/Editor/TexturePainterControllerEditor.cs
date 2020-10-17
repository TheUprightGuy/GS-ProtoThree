using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;

[CustomEditor(typeof(TexturePainterController))]
public class TexturePainterControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TexturePainterController t = target as TexturePainterController;

        if (GUILayout.Button("ReGen"))
        {
            t.UpdateDefaultColors();
        }
    }
     void OnSceneGUI()
     {
     
         Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
         RaycastHit hit;

        TexturePainterController t = target as TexturePainterController;
        Handles.color = Color.white;
         if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Island")) && !Event.current.control)
         {
             int id = GUIUtility.GetControlID(FocusType.Passive);
             HandleUtility.AddDefaultControl(id);
             Tools.hidden = true;

            Handles.Disc(Quaternion.identity, ray.origin + (ray.direction * 10.0f), ray.direction, 1, false, 1);
            t.Hitpoint = hit.point;
             //SHandles.Disc(Quaternion.identity, t.transform.position, new Vector3(1, 1, 0), 5, false, 1);
         }
         else
         {
             Tools.hidden = false;
             t.Hitpoint = Vector3.positiveInfinity;
         }
     
         if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
         {
             Debug.Log("Left-Mouse Down");
            t.UpdateBrush();
         }
     
         //if (t.ModifyVoxels())
         //{
         //    Handles.color = Color.red;
         //}
         
         SceneView.RepaintAll();
     }
}
