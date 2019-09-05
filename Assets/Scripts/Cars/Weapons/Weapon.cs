using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    LineRenderer lineRenderer;

    public float fuelPerBarrelTossed = 10;
    public Transform spawnPoint; // this is a reference to where the projectiles would spawn
    public Transform cursor; // this is a reference to where the weapon is aiming
    public AnimationCurve curve;
    float aimMaxDistance = 15;
    float arcHeight = 2;

    public void AimAt(Vector3 pos) {

        // clamp to a max distance:
        Vector3 dis = pos - transform.position;
        float maxLateralDistance = 20;
        float disMag = dis.magnitude;
        if(disMag > maxLateralDistance) {
            dis *= maxLateralDistance / disMag;
        }

        cursor.position = transform.position + dis;

        DrawAimPath();
    }
    
    void DrawAimPath() {
        if (lineRenderer == null) lineRenderer = GetComponentInChildren<LineRenderer>();
        if (lineRenderer == null) return;

        int resolution = 10;
        Vector3[] arc = GetArc(resolution);

        lineRenderer.positionCount = resolution;
        lineRenderer.SetPositions(arc);

    }

    public Vector3[] GetArc(int resolution, bool localSpace = true) {
        
        Vector3[] pts = new Vector3[resolution];

        int max = pts.Length - 1;
        for (int i = 0; i < pts.Length; i++) {
            float p = i / (float)max;
            pts[i] = GetArcAt(p);
            if (localSpace) {
                pts[i] = spawnPoint.InverseTransformPoint(pts[i]);
            }
        }

        return pts;
    }
    public Vector3 GetArcAt(float p) {
        Vector3 pt = Vector3.Lerp(spawnPoint.position, cursor.position, p);
        pt.y += curve.Evaluate(p) * arcHeight;
        return pt;
    }
    public abstract void FireWeapons(Driver shooter);
}
