using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {


    public float attackCost = 5;
    public bool infiniteFuel = false;
    public float maximumFuel = 100;
    public float currentFuel { get; private set; }

    TurretRotation weapon;

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

    LineRenderer lineRenderer;

    public Transform aiSteerVisual;
    public TextMesh text;

    public ParticleSystem[] dustParticles;
    public AnimationCurve boostFalloff;

    public float throttleMin = 800;
    public float throttleMax = 2000;

    private float health = 100;

    [HideInInspector] public Driver driver;
    CarState state;

    void Start() {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        ballBody = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<TurretRotation>();

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
    void Update()
    {
        if (driver != null) driver.Drive();
        SwitchState(state.Update());
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
    public void Boost()
    {
        float p = ballBody.velocity.sqrMagnitude / 10000;
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
        turnAmount = amount * state.turnMultiplier;
        text.text = turnAmount.ToString();
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
}
