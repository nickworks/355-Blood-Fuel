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

        if (car.currentFuel > 1) score += Time.fixedDeltaTime;

        float h = Input.GetAxisRaw("Steering");
        float t = Input.GetAxis("Gas");

        car.SetThrottle(t);
        car.Turn(h);

        if (car.weapon != null) {
            //AimWithMouse();
            AimWithAnalog();
        }
    }
    public override void DriveUpdate() {
        if (Input.GetButtonDown("Jump")) car.Jump();
        if (Input.GetButton("FireWeapons")) car.FireWeapons();
        if (Input.GetButton("Boost")) car.Boost();
        if (Input.GetButton("Handbrake")) car.Handbrake();
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
        
        float aimAxisH = Input.GetAxis("Horizontal2");
        float aimAxisV = Input.GetAxis("Vertical2");

        

        //PlayerController.main.line.enabled = Input.GetButton("BumperRight");
        //PlayerController.main.line.enabled = (trigger < -.2f);

        Vector3 target = new Vector3(aimAxisH, 0, aimAxisV);
        if (target.sqrMagnitude > 1) target.Normalize();

        wantsToAim = (target.sqrMagnitude > .25f); // deadZone
        if (!wantsToAim) {
            target = car.ballBody.velocity.normalized;
        }
        float aimMaxDistance = 6; // the farthest the player can aim
        target *= aimMaxDistance;
        //if (aimAxisV > 0) target.z *= 2;

        //DriverPlayer.main.line.enabled = !deadZone; // TEMP DISABLE

        target += car.transform.position;

        car.weapon.AimAt(target);
    }

    public override void OnDestroy(bool isDead) {
        PlayerManager.Remove(this, isDead);
    }
}
