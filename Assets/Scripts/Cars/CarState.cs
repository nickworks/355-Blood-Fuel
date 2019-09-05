using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarState {

    protected Car car;

    public float throttleMultiplier { get; protected set; }
    public float turnMultiplier { get; protected set; }

    public Vector3 forward { get; private set; }
    public Vector3 up { get; private set; }

    public Quaternion suspensionOrientation { get; protected set; }
    public float suspensionRotateSpeed { get; protected set; }

    public abstract CarState Update();

    public virtual void OnStart(Car car) {
        this.car = car;
        CalcOrientation();
    }
    public virtual void OnEnd() {
        this.car = null;
    }
    protected virtual bool DetectGround(out string materialName) {
        materialName = "";

        RaycastHit hit;
        bool isGrounded = (Physics.Raycast(car.transform.position, Vector3.down, out hit, 1.0f));
        if (isGrounded)
        {
            materialName = hit.collider.material.name;
        }
        forward = isGrounded ? Vector3.Cross(Vector3.right, up) : Vector3.forward;
        up = isGrounded ? hit.normal : Vector3.up;
        return isGrounded;
    }

    protected abstract void CalcOrientation();
    
}
public class CarStateGround : CarState {
    public CarStateGround() {
        throttleMultiplier = 1;
        turnMultiplier = 1;
    }
    override public CarState Update() {
        CalcOrientation();

        bool isGrounded = DetectGround(out string material);

        bool onSand = (material == "Sand (Instance)");
        car.SandParticles(onSand ? 50 : 0);

        return isGrounded ? null : new CarStateAir();
    }
    public override void OnEnd() {
        car.SandParticles(0);
    }
    protected override void CalcOrientation() {
        suspensionOrientation = Quaternion.FromToRotation(Vector3.up, up);
        suspensionRotateSpeed = 180;
    }
}
public class CarStateAir : CarState {
    public CarStateAir() {
        throttleMultiplier = 0.75f;
        turnMultiplier = 0.75f;
    }

    override public CarState Update() {
        CalcOrientation();
        bool isGrounded = DetectGround(out string material);
        return isGrounded ?  new CarStateGround() : null;
    }
    protected override void CalcOrientation() {
        float pitch = -car.ballBody.velocity.y * 2;
        suspensionOrientation = Quaternion.Euler(pitch, 0, 0);
        suspensionRotateSpeed = 40;
    }
}