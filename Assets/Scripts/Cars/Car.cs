using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {


    public float attackCost = 5;
    public bool infiniteFuel = false;
    public float maximumFuel = 100;
    public float currentFuel { get; private set; }

    public Weapon weapon { get; private set; }

    /// <summary>
    /// The ball that is the car. Think of it as a hamster ball.
    /// </summary>
    public Rigidbody ballBody { get; private set; }

    /// <summary>
    /// This is the orientation of the tires. -1 to 1
    /// -1 is left, and +1 is right.
    /// </summary>
    public float turnAmount { get; private set; }
    public float throttle { get; private set; }

    /// <summary>
    /// This script will snap the position's position to the ball,
    /// and orient the suspension to the ground.
    /// </summary>
    public Transform suspension;
    /// <summary>
    /// The model should be a child of suspension.
    /// It will turn on its local yaw to animate turning.
    /// </summary>
    public Transform model;

    public LineRenderer lineRenderer;

    public Transform aiSteerVisual;
    public TextMesh text;

    public ParticleSystem[] dustParticles;
    public AnimationCurve boostFalloff;

    public float throttleMin = 800;
    public float throttleMax = 2000;

    public float maxSpeed = 200;
    private float health = 100;

    [HideInInspector] public Driver driver;
    CarState state;

    void Start() {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        ballBody = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<Weapon>();

        currentFuel = maximumFuel;
        SwitchState(new CarStateGround());
    }
    public void SetLine(Vector3 start, Vector3 end, Color color = default) {
        if (color == default) color = Color.black;

        lineRenderer.material.color = color;

        Vector3[] pts = { start, end };
        lineRenderer.SetPositions(pts);
    }
    public void InitSpeed(Car car) {
        ballBody = GetComponent<Rigidbody>();
        ballBody.velocity = car.ballBody.velocity;
    }
    private void SwitchState(CarState newCS)
    {
        if (newCS == null) return;
        if (state != null) state.OnEnd();
        state = newCS;
        newCS.OnStart(this);
    }
    void FixedUpdate() {
        if (driver != null) driver.Drive();
    }
    void Update()
    {
        SwitchState(state.Update());

        MoveCar();
        UpdateModel();

        if (health <= 0) Destroy(gameObject);
    }
    public void Kill(bool killSilently = false) {
        health = 0;
        Destroy(gameObject);
    }
    void OnDestroy() {
        if (driver != null) driver.OnDestroy((health <= 0));
    }
    public void AddFuel(float delta)
    {
        currentFuel += delta;
        currentFuel = Mathf.Clamp(currentFuel, 0, maximumFuel);
    }
    public void Jump()
    {
        ballBody.AddForce(Vector3.up * 20, ForceMode.Impulse);
    }
    public void Boost() {
        float p = ballBody.velocity.z / maxSpeed;
        float m = boostFalloff.Evaluate(p);
        ballBody.AddForce(model.forward * 5000 * m * Time.deltaTime);
        model.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
    }
    public void SetThrottle(float throttlePercent) {
        if (throttlePercent < 0) throttlePercent = 0;
        throttle = Mathf.Lerp(throttleMin, state.throttleMultiplier * throttleMax, throttlePercent);
        AddFuel(-throttlePercent * Time.deltaTime); // lose 1 fuel per second
    }
    public void Turn(float amount) {
        turnAmount = amount;
    }
    public void FireWeapons() {
        if (weapon != null) weapon.FireWeapons();
    }
    public void SandParticles(float amt)
    {
        SetParticleRate(dustParticles, amt);
    }
    void SetParticleRate(ParticleSystem[] ps, float perSecond)
    {
        foreach (ParticleSystem p in ps)
        {
            var em = p.emission;
            em.rateOverTime = perSecond;
            //em.rateOverDistance = overDistance;
        }
    }

    private void MoveCar() {
        if (currentFuel <= 0) return;
        
        // move forward:
        ballBody.AddForce(state.forward * throttle * state.throttleMultiplier * Time.deltaTime);

        // move horizontal:

        // TODO: refactor
        // this is a good step, but I think I should try
        // using an angle and then derive horizontal velocity
        // off of forward velocity and the angle

        if(turnAmount == 0) {            
            ballBody.velocity = DecelerateHorizontal(ballBody.velocity);
        } else {
            ballBody.velocity = AccelerateHorizontal(ballBody.velocity, turnAmount);
        }
        text.text = "" + turnAmount;
        
        /*
        float wheelAngleMax = 45;
        float angle = 0; 
        if(turnAmount < 0) angle = Mathf.Lerp(0, -wheelAngleMax, -turnAmount);
        else angle = Mathf.Lerp(0, wheelAngleMax, turnAmount);

        if (model) {
            model.localEulerAngles = new Vector3(0, angle, 0);
            ballBody.AddForce(model.forward * throttle * state.throttleMultiplier * Time.deltaTime);
        }
        text.text = "" + angle;
        */
    }
    private Vector3 AccelerateHorizontal(Vector3 velocity, float direction) {
        velocity.x += 100 * direction * state.turnMultiplier * Time.deltaTime;
        float maxHorizontalSpeed = 100;
        if (velocity.x > maxHorizontalSpeed) velocity.x = maxHorizontalSpeed;
        if (velocity.x < -maxHorizontalSpeed) velocity.x = -maxHorizontalSpeed;
        return velocity;
    }
    private Vector3 DecelerateHorizontal(Vector3 velocity) {
        float decelMultiplier = .75f;
        /*
        if(velocity.x > 0) {
            velocity = AccelerateHorizontal(velocity, -decelMultiplier);
            if (velocity.x < 0) velocity.x = 0;
        }
        if (velocity.x < 0) {
            velocity = AccelerateHorizontal(velocity, decelMultiplier);
            if (velocity.x > 0) velocity.x = 0;
        }
        */
        velocity.x *= .99f; // TODO: make framrate-independent
        return velocity;
    }
    public void UpdateModel() {

        // rotate the suspension to align with provided rotation:
        suspension.position = transform.position; // make the model follow the hamster wheel! ////////////////////// NOTE: If suspension is a child of the veichle do we need thi? 
        suspension.rotation = Quaternion.RotateTowards(suspension.rotation, state.suspensionOrientation, state.suspensionRotateSpeed * Time.deltaTime);

        // rotate the car model to align with velocity:
        
        if (model) {
            Vector3 vel = ballBody.velocity;
            float turn = Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg;
            model.localEulerAngles = new Vector3(0, turn, 0);
        }
        
    }
}
