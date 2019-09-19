using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelLauncher : Weapon {

    LineRenderer lineRenderer;

    public GameObject prefabBarrel;
    public AnimationCurve arcCurve;
    float arcHeight = 2;
    public override void AimAt(Vector3 pos) {
        base.AimAt(pos);
        DrawAimPath();
    }
    public override void FireWeapons() {

        if (car.currentFuel < fuelPerShot) return;
        //car.AddFuel(-fuelPerBarrelTossed); // lose fuel
        ShootProjectile(prefabBarrel);
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
        pt.y += arcCurve.Evaluate(p) * arcHeight;
        return pt;
    }
    public void ShootProjectile(GameObject prefab) {

        // random rotation:
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, Random.onUnitSphere);

        // spwn the barrel:
        GameObject obj = Instantiate(prefab, spawnPoint.position, rot);

        // setup who the shooter is:
        obj.GetComponent<ProjectileController>().SetShooter(car.driver);

        // set arc (projectile) physics:
        obj.GetComponent<PhysicsArcing>().SetArc(spawnPoint.position, car.ballBody.velocity, GetArc(10), .5f);

        // set random spin:
        obj.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * 100);
    }
}

