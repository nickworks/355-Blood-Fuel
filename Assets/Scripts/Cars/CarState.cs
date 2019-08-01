using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarState {

    protected Car car;

    public float throttleMultiplier = 1;
    public float turnMultiplier = 1;

    public Vector3 forward { get; private set; }
    public Vector3 up { get; private set; }

    public abstract CarState Update();

    public virtual void OnStart(Car car) {
        this.car = car;
    }
    public virtual void OnEnd() {
        this.car = null;
    }
    protected virtual bool DetectGround(out string materialName)
    {
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
    protected void Drive() {
        if (car.currentFuel <= 0) return;

        // move forward:
        car.ballBody.AddForce(forward * car.throttle * Time.deltaTime);

        // move side-to-side:
        Vector3 vel = car.ballBody.velocity;
        vel.x = car.ballBody.velocity.x + 100 * car.turnAmount * Time.deltaTime;

        float maxHorizontalSpeed = 100;
        if (vel.x > maxHorizontalSpeed) vel.x = maxHorizontalSpeed;
        if (vel.x < -maxHorizontalSpeed) vel.x = -maxHorizontalSpeed;

        car.ballBody.velocity = vel;
    }
    protected virtual void UpdateModel() {
        UpdateModelRotation(Quaternion.FromToRotation(Vector3.up, up), 180);
    }
    protected void UpdateModelRotation(Quaternion suspensionRotation, float rotateSpeed) {

        // rotate the suspension to align with provided rotation:
        car.suspension.position = car.transform.position; // make the model follow the hamster wheel! ////////////////////// NOTE: If suspension is a child of the veichle do we need thi? 
        car.suspension.rotation = Quaternion.RotateTowards(car.suspension.rotation, suspensionRotation, rotateSpeed * Time.deltaTime);

        // rotate the car model to align with velocity:
        if (car.model) {
            Vector3 vel = car.ballBody.velocity;
            float turn = Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg;
            car.model.localEulerAngles = new Vector3(0, turn, 0);
        }
    }
}
public class CarStateGround : CarState {
    public CarStateGround() {
    }
    override public CarState Update() {
        bool isGrounded = DetectGround(out string material);

        bool onSand = (material == "Sand (Instance)");
        car.SandParticles(onSand ? 50 : 0);

        Drive();
        UpdateModel();

        return isGrounded ? null : new CarStateAir();
    }
    public override void OnEnd() {
        car.SandParticles(0);
    }
}
public class CarStateAir : CarState {
    public CarStateAir() {
        throttleMultiplier = 0.75f;
        turnMultiplier = .1f;
    }

    override public CarState Update() {
        bool isGrounded = DetectGround(out string material);
        Drive();
        UpdateModel();
        return isGrounded ?  new CarStateGround() : null;
    }
    protected override void UpdateModel() {
        float pitch = -car.ballBody.velocity.y * 2;
        UpdateModelRotation(Quaternion.Euler(pitch, 0, 0), 40);
    }
}