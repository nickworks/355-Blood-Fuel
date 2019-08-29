using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

/// <summary>
/// This class creates custom Editor GUI (scene and inspector) and behavior for PathNode objects.
/// </summary>
[CustomEditor(typeof(DrivePath))]
[CanEditMultipleObjects]
public class DrivePathEditor : Editor
{
    public static bool globalTransform { get; private set; }
    /// <summary>
    /// This method draws the Inspector GUI for PathNode objects.
    /// </summary>
    override public void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        DrivePath path = ((DrivePath)target);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("( + )\nPush Point")) {
            path.AddPoint();
        }
        if (GUILayout.Button("( - )\nPop Point")) {
            path.RemovePoint();  
        }
        GUILayout.EndHorizontal();

        DrivePathEditor.globalTransform = EditorGUILayout.Toggle("Global Transform", DrivePathEditor.globalTransform);
        
        bool pre = DrivePath.renderLines;
        bool post = EditorGUILayout.Toggle("Render All Paths", pre);
        if(pre != post) {
            DrivePath.renderLines = post;
            UnityEngine.Object[] paths = Resources.FindObjectsOfTypeAll(typeof(DrivePath));
            foreach (UnityEngine.Object p in paths) {
                // turn on or off all LineRenderers on DrivePaths
                (p as DrivePath).GetComponent<LineRenderer>().enabled = DrivePath.renderLines;
            }
        }


        if (GUI.changed) {
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews(); // force a redraw...
        }
    }

    void OnSceneGUI()
    {
        DrivePath dp = (DrivePath)target;
        
        EditorGUI.BeginChangeCheck();
        if (!DrivePathEditor.globalTransform) // local mode
        {
            Tools.current = Tool.None;
            Vector3[] pos = new Vector3[dp.points.Count];
            for (int i = 0; i < pos.Length; i++) {
                // create handles:
                // this also stores the position of the handle (in case it moved)
                pos[i] = Handles.PositionHandle(dp.WorldPoint(i), Quaternion.identity);
            }
            if (EditorGUI.EndChangeCheck()) { // if some value was changed:
                Undo.RecordObject(target, "Updated drive path"); // register a new "undo"
                for (int i = 0; i < pos.Length; i++) {
                    dp.SetFromWorld(i, pos[i]); // set point from position handle
                }
                dp.Refresh(); // update line renderer
            }
        } else // global mode:
        {
            if (Tools.current == Tool.None) Tools.current = Tool.Move;
            EditorGUI.EndChangeCheck(); // business as usual: move / rotate gameobject
        }
    } // ends OnSceneGUI()
}