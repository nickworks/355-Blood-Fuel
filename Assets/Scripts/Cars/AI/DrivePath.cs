using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DrivePath : MonoBehaviour
{
    /// <summary>
    /// Path points in local space
    /// </summary>
    [HideInInspector]
    public List<Vector3> points = new List<Vector3>() { Vector3.zero };

    private static List<DrivePath> registeredPaths = new List<DrivePath>();

    private LineRenderer lines;

    public static Vector3 ProjectToNearestPath(Vector3 pos)
    {
        Vector3 result = pos;
        float currDistance = float.PositiveInfinity;

        for(int i = registeredPaths.Count - 1; i >= 0; i--)
        {
            if(registeredPaths[i] == null)
            {
                registeredPaths.RemoveAt(i);
            } else
            {
                
                Vector3 proj = registeredPaths[i].ProjectPoint(pos);
                if (proj.z < pos.z) continue; // if the projected point is behind us, ignore it
                float dis = (proj - pos).sqrMagnitude;
                if (dis < currDistance)
                {
                    currDistance = dis;
                    result = proj;
                }
            }
        }


        return result;
    }

    void Start()
    {
        Refresh();
        registeredPaths.Add(this);
    }
    void OnValidate()
    {
        Refresh();
    }
    public void Refresh()
    {
        lines = GetComponent<LineRenderer>();
        lines.positionCount = points.Count;
        lines.SetPositions(points.ToArray());
    }
    void Update()
    {
        
    }
    void OnDrawGizmos()
    {
        DrawLines();
    }
    public Vector3[] WorldPoints()
    {
        Vector3[] worldpoints = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++) worldpoints[i] = transform.TransformPoint(points[i]);
        return worldpoints;
    }
    public Vector3 WorldPoint(int i)
    {
        if (i < 0 || i >= points.Count) return Vector3.zero;
        return transform.TransformPoint(points[i]);
    }
    public void SetFromWorld(int i, Vector3 p)
    {
        if (i < 0 || i >= points.Count) return;
        if (i < 0 || i >= points.Count) return;
        points[i] = transform.InverseTransformPoint(p);
    }
    void DrawLines()
    {
        Gizmos.DrawIcon(transform.position, "icon-path.png", true);

        Vector3[] worldpoints = WorldPoints();
        for (int i = 0; i < worldpoints.Length; i++)
        {
            Gizmos.DrawCube(worldpoints[i], Vector3.one * .5f);
            if (i > 0)
            {
                Gizmos.DrawLine(worldpoints[i], worldpoints[i - 1]);
            }
        }
    }
    public void AddPoint()
    {
        Vector3 endPt = points[points.Count - 1];
        points.Add(endPt + new Vector3(0, 0, 20));
    }
    public void RemovePoint()
    {
        if (points.Count == 1) return;
        points.RemoveAt(points.Count - 1);
    }
    public Vector3 ProjectPoint(Vector3 pt)
    {
        Vector3[] worldPoints = WorldPoints();

        if (worldPoints[0].z > pt.z) return worldPoints[0]; // if the whole path is ahead of the player, return the first point
        if (worldPoints[worldPoints.Length - 1].z < pt.z) return worldPoints[worldPoints.Length - 1];

        for (int i = 0; i < worldPoints.Length; i++) { // go through each point:
            if (worldPoints[i].z < pt.z) continue; // if this point is behind the player, ignore this point
            Vector3 p1 = worldPoints[i];
            Vector3 p2 = (i == 0) ? worldPoints[worldPoints.Length - 1] :  worldPoints[i - 1];
            float range = p1.z - p2.z;
            float p = (pt.z - p2.z) / range;
            Vector3 proj = Vector3.Lerp(p2, p1, p);
            pt.x = proj.x;
            break;
        }

        return pt;
    }
}