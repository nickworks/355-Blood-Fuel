using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarState {

    protected Car car;

    public bool isGrounded { get; private set; }
    public bool onSand { get; private set; }
    public Vector3 forward { get; private set; }
    public Vector3 up { get; private set; }

    public abstract CarState Update();

    public virtual void OnStart(Car car) {
        this.car = car;
    }
    public virtual void OnEnd() {
        this.car = null;
    }
    protected virtual void DetectGround()
    {
        RaycastHit hit;
        isGrounded = (Physics.Raycast(car.transform.position, Vector3.down, out hit, 1.0f));
        if (isGrounded)
        {
            onSand = (hit.collider.material.name == "Sand (Instance)");
        }
        forward = isGrounded ? Vector3.Cross(Vector3.right, up) : Vector3.forward;
        up = isGrounded ? hit.normal : Vector3.up;
    }
    protected void Drive()
    {
        if (car.currentFuel <= 0) return;

        Vector3 driveDir = forward + new Vector3(car.turnAmount, 0, 0);
        //Debug.DrawRay(car.transform.position, driveDir);

        car.ballBody.AddForce(driveDir * car.throttle * Time.deltaTime);
    }
    protected void UpdateModel(Quaternion suspensionRotation)
    {
        float rotateSpeed = isGrounded ? 180 : 40; // the maximum number of degrees to rotate per second

        car.suspension.position = car.transform.position; // make the model follow the hamster wheel! ////////////////////// NOTE: If suspension is a child of the veichle do we need thi? 
        car.suspension.rotation = Quaternion.RotateTowards(car.suspension.rotation, suspensionRotation, rotateSpeed * Time.deltaTime);
        if (car.model)
        {
            Vector3 vel = car.ballBody.velocity;
            float turn = Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg;
            car.model.localEulerAngles = new Vector3(0, turn, 0);
        }
    }
}
public class CarStateGround : CarState
{
    public CarStateGround()
    {
        //Debug.Log("on ground");
    }
    override public CarState Update()
    {
        DetectGround();
        car.AddFuel(-1 * Time.deltaTime); // lose 1 fuel per second

        Drive();
        
        car.SandParticles(onSand ? 50 : 0);
        UpdateModel(Quaternion.FromToRotation(Vector3.up, up));
        if (!isGrounded) return new CarStateAir();
        return null;
    }
    public override void OnEnd()
    {
        car.SandParticles(0);
    }
}
public class CarStateAir : CarState
{
    public CarStateAir()
    {
        //Debug.Log("in air");
    }
    override public CarState Update()
    {
        DetectGround();
        car.AddFuel(-1 * Time.deltaTime); // lose 1 fuel per second
        Drive();
        float pitch = -car.ballBody.velocity.y * 2;
        UpdateModel(Quaternion.Euler(pitch, 0, 0));
        if (isGrounded) return new CarStateGround();
        return null;
    }
}