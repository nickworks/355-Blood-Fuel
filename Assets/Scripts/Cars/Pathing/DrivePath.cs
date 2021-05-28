using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(LineRenderer))]
public class DrivePath : MonoBehaviour
{
    /// <summary>
    /// A list of all DrivePaths currently in the world.
    /// </summary>
    private static List<DrivePath> registeredPaths = new List<DrivePath>();
    public static bool renderLines = true;

    public bool spawnLoot = true;
    public Transform prefabLoot;

    /// <summary>
    /// Path points in local space
    /// </summary>
    [HideInInspector]
    public List<Vector3> points = new List<Vector3>() { Vector3.zero };

    private LineRenderer lines;

    /// <summary>
    /// Given a world position, this function finds the nearest point on ANY path.
    /// </summary>
    /// <param name="pos">The world position to search from.</param>
    /// <returns>The world position of a point on a path. If a path couldn't be found, the supplied point is returned.</returns>
    public static Vector3 ProjectToNearestPath(Vector3 pos, out bool pathWasFound) {
        pathWasFound = false;
        Vector3 result = pos;
        float currDistance = float.PositiveInfinity;

        for(int i = registeredPaths.Count - 1; i >= 0; i--) {
            if(registeredPaths[i] == null) { // referenced path is null
                registeredPaths.RemoveAt(i);
                print("good, a path was removed...");
                continue; // don't do the following
            }
            Vector3 proj = registeredPaths[i].ProjectPoint(pos);
            if (proj.z < pos.z) continue; // if the projected point is behind us, ignore it
            float dis = (proj - pos).sqrMagnitude;
            if (dis < currDistance) {
                currDistance = dis;
                result = proj;
                pathWasFound = true;
            }
        }
        return result;
    }

    void Start() {
        Refresh();
        if (spawnLoot) SpawnLoot();
        registeredPaths.Add(this);
    }

    private void SpawnLoot() {
        if (!prefabLoot) return;
        foreach(Vector3 pt in points) {
            Vector3 wp = transform.TransformPoint(pt);
            Ray ray = new Ray(wp + new Vector3(0, 5, 0), Vector3.down);
            if(Physics.Raycast(ray, out RaycastHit hit, 50)) {
                Instantiate(prefabLoot, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
        }
    }

    void OnDestroy() {
        registeredPaths.Remove(this);
    }
    void OnValidate() {
        Refresh();
    }
    /// <summary>
    /// This method redraws the LineRenderer.
    /// </summary>
    public void Refresh() {
        lines = GetComponent<LineRenderer>();
        lines.useWorldSpace = false;
        lines.positionCount = points.Count;
        lines.SetPositions(points.ToArray());
        lines.enabled = renderLines;
    }
    void OnDrawGizmos() {
        DrawLines();
    }
    /// <summary>
    /// Returns this path as an array of world-positions.
    /// </summary>
    /// <returns></returns>
    public Vector3[] WorldPoints() {
        Vector3[] worldpoints = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++) worldpoints[i] = transform.TransformPoint(points[i]);
        return worldpoints;
    }
    /// <summary>
    /// Returns the world-position in the array at a specified index.
    /// </summary>
    /// <param name="i">The index number to look up.</param>
    /// <returns></returns>
    public Vector3 WorldPoint(int i) {
        if (i < 0 || i >= points.Count) return Vector3.zero;
        return transform.TransformPoint(points[i]);
    }
    /// <summary>
    /// Sets the position of one of the points. This should mostly be called from the inspector.
    /// </summary>
    /// <param name="i">The index number of the point to change.</param>
    /// <param name="p">The world-position to move the point to.</param>
    public void SetFromWorld(int i, Vector3 p) {
        if (i < 0 || i >= points.Count) return;
        if (i < 0 || i >= points.Count) return;
        points[i] = transform.InverseTransformPoint(p);
    }
    /// <summary>
    /// Draws the path using Gizmo lines (editor only).
    /// </summary>
    void DrawLines() {
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
    /// <summary>
    /// Adds a new point to the end of the path.
    /// </summary>
    public void AddPoint() {
        Vector3 endPt = points[points.Count - 1];
        points.Add(endPt + new Vector3(0, 0, 20));
    }
    /// <summary>
    /// Removes a point from the end of the path.
    /// </summary>
    public void RemovePoint() {
        if (points.Count == 1) return;
        points.RemoveAt(points.Count - 1);
    }
    /// <summary>
    /// Finds the closest point on this path to a specified point.
    /// </summary>
    /// <param name="pt">The world-position point to search from.</param>
    /// <returns>The corresponding position on the path.</returns>
    public Vector3 ProjectPoint(Vector3 pt) {
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