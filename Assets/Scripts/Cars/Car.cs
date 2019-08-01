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

    public Transform aiSteerVisual;

    public ParticleSystem[] dustParticles;
    public AnimationCurve boostFalloff;

    public float throttleMin = 800;
    public float throttleMax = 2000;
    public float throttleMaxAir = 1500;
    public float turnMultiplier = 1;

    public float health = 100;

    [HideInInspector] public Driver driver;
    CarState state;

    void Start()
    {
        ballBody = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<TurretRotation>();

        currentFuel = maximumFuel;
        SwitchState(new CarStateGround());
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
    private void OnDestroy() {
        if (driver != null) driver.OnDestroy();
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
    public void SetThrottle(float t)
    {
        if (t < 0) t = 0;
        throttle = Mathf.Lerp(throttleMin, state.isGrounded ? throttleMax : throttleMaxAir, t);
    }
    public void Turn(float amount)
    {
        turnAmount = amount * turnMultiplier;
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
