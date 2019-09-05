using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverPlayer : Driver {

    public float score = 0;

    public DriverPlayer() {
        score = 0;
    }
    override public void Drive() {
        if (car == null) return;

        //AddFuel(-1 * Time.deltaTime); // lose 1 fuel per second
        if (car.currentFuel > 1) score += Time.deltaTime;

        float h = Input.GetAxis("Horizontal");
        float t = Input.GetAxis("Triggers");

        float v = Input.GetAxis("Vertical");

        car.SetThrottle(Mathf.Max(-t, v));
        car.Turn(h);

        if (Input.GetButtonDown("Jump")) car.Jump();
        if (Input.GetButtonDown("Fire1")) car.FireWeapons();
        if (Input.GetButton("Fire2")) car.Boost();
    }

    public override void OnDestroy(bool isDead) {
        PlayerManager.Remove(this, isDead);
    }
}
