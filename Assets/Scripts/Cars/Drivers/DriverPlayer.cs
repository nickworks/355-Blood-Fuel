using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverPlayer : Driver {

    public float score = 0;

    public DriverPlayer() {
        score = 0;
    }
    /// <summary>
    /// The Car calls it's Player's Drive() method every game tick.
    /// </summary>
    override public void DriveFixedUpdate() {
        
        if (car == null) return;

        //AddFuel(-1 * Time.deltaTime); // lose 1 fuel per second
        if (car.currentFuel > 1) score += Time.fixedDeltaTime;

        float h = Input.GetAxisRaw("Horizontal");
        float t = Input.GetAxis("Triggers");

        float v = Input.GetAxis("Vertical");

        car.SetThrottle(Mathf.Max(-t, v));
        car.Turn(h);

        if (car.weapon != null) {
            AimWithMouse();
        }
    }
    public override void DriveUpdate() {
        if (Input.GetButtonDown("Jump")) car.Jump();
        if (Input.GetButtonDown("Fire1")) car.FireWeapons();
        if (Input.GetButton("Fire2")) car.Boost();
    }
    void AimWithMouse() {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // make a ray
        Plane aimPlane = new Plane(car.state.up, car.transform.position); // make a plane

        float rayLength = 0;
        if (aimPlane.Raycast(ray, out rayLength)) // detect if the ray intersects the plane
        {
            Vector3 hit = ray.GetPoint(rayLength); // detect where the intersection is

            car.weapon.AimAt(hit);
        }
    }
    void AimWithAnalog() {
        /*
        float aimAxisH = Input.GetAxis("Horizontal2");
        float aimAxisV = Input.GetAxis("Vertical2");

        float trigger = Input.GetAxis("Triggers");

        //PlayerController.main.line.enabled = Input.GetButton("BumperRight");
        //PlayerController.main.line.enabled = (trigger < -.2f);

        Vector3 target = new Vector3(aimAxisH, 0, aimAxisV);
        if (target.sqrMagnitude > 1) target.Normalize();

        bool deadZone = (target.sqrMagnitude < .1f); // deadZone
        target *= aimMaxDistance;
        if (aimAxisV > 0) target.z *= 2;

        //DriverPlayer.main.line.enabled = !deadZone; // TEMP DISABLE

        if (deadZone) return;

        bool aimFurtherOut = (target.sqrMagnitude >= cursor.localPosition.sqrMagnitude);
        float inputAlignAmount = Vector3.Dot(target, cursor.localPosition);
        bool letsAimThisThing = (aimFurtherOut || inputAlignAmount < .5f || target.sqrMagnitude > .8f);

        if (letsAimThisThing) {
            cursor.localPosition += (target - cursor.localPosition) * Time.deltaTime * 4;
        }
        cursor.rotation = Quaternion.identity;
        */
    }

    public override void OnDestroy(bool isDead) {
        PlayerManager.Remove(this, isDead);
    }
}
