using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    LineRenderer lineRenderer;

    public float fuelPerBarrelTossed = 10;
    public Transform spawnPoint;
    public Transform cursor;
    public AnimationCurve curve;
    float aimMaxDistance = 15;

    public void AimAt(Vector3 pos) {
        cursor.position = pos;
        DrawAimPath();
    }
    
    void DrawAimPath() {
        if (lineRenderer == null) lineRenderer = GetComponentInChildren<LineRenderer>();
        if (lineRenderer == null) return;

        Vector3[] pts = new Vector3[lineRenderer.positionCount];
        
        float height = 2;
        
        for (int i = 0; i < pts.Length; i++)
        {
            int max = pts.Length - 1 + 4;
            float p = i / (float)max;

            //p *= .6f;
            Vector3 pt = Vector3.Lerp(transform.position, cursor.position, p);
            pt.y += curve.Evaluate(p) * height;
            pts[i] = pt;
            if(i == 1)
            {
                //spawnPoint.position = pt;
            }
        }
        
        lineRenderer.SetPositions(pts);
        
    }
    public abstract void FireWeapons();
}
