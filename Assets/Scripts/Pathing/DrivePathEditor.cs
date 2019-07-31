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
        if (GUILayout.Button("Push + "))
        {
            path.AddPoint();
        }
        if (GUILayout.Button("Pop - "))
        {
            path.RemovePoint();  
        }
        GUILayout.EndHorizontal();

        DrivePathEditor.globalTransform = EditorGUILayout.Toggle("Global Transform", DrivePathEditor.globalTransform);
        if (GUI.changed)
        {
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews(); // force a redraw...
        }
    }
    /// <summary>
    /// This method renames all of the PathNode objects in a path.
    /// </summary>
    /// <param name="path">A node from the desired path.</param>
    void Rename(DrivePath path)
    {
        if (!path) return;
        //path.GetLeftMostNode().RenameNodes("Node");
    }
    void OnSceneGUI()
    {
        DrivePath dp = (DrivePath)target;
        
        EditorGUI.BeginChangeCheck();
        if (!DrivePathEditor.globalTransform) // local mode
        {
            Tools.current = Tool.None;
            Vector3[] pos = new Vector3[dp.points.Count];
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = Handles.PositionHandle(dp.WorldPoint(i), Quaternion.identity);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Updated drive path");
                for (int i = 0; i < pos.Length; i++)
                {
                    dp.SetFromWorld(i, pos[i]);
                }
                dp.Refresh();
            }
        } else // global mode:
        {
            if (Tools.current == Tool.None) Tools.current = Tool.Move;
            EditorGUI.EndChangeCheck();
        }
    } // ends OnSceneGUI()
}