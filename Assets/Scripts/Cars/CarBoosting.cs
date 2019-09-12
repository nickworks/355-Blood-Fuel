using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class CarBoosting : MonoBehaviour
{
    /// <summary>
    /// The number of seconds of boost that should be available when boostAmount is at 100%.
    /// </summary>
    public float secondsOfBoostTotal;
    /// <summary>
    /// The total number of seconds left before boost starts regenerating.
    /// </summary>
    public float timeBeforeRecharge = 2;
    public float rechargeSpeed = 1;
    public ParticleSystem boostParticles;
    public AnimationCurve boostFalloff;


    /// <summary>
    /// The number of seconds of boost remaining.
    /// </summary>
    public float secondsOfBoostCurrent { get; private set; }
    /// <summary>
    /// The current number of seconds left before boost starts regenerating.
    /// </summary>
    private float timeBeforeRechargeCurrent = 0;

    public bool isBoosting { get; private set; }

    private Car car;

    void Start() {
        car = GetComponent<Car>();
    }
    void LateUpdate() {    
        SetParticleRate(boostParticles, isBoosting ? 100 : 0); // set particle rate for this frame
        isBoosting = false; // turn off for next frame
        BoostRestock();
    }
    public void Boost() {

        if (secondsOfBoostCurrent <= 0) return; // exit if no boost available

        secondsOfBoostCurrent -= Time.deltaTime;

        float p = car.ballBody.velocity.z / car.maxSpeed;
        float m = boostFalloff.Evaluate(p);

        // TODO: rewrite this to avoid boosting up / down in air (??)
        car.ballBody.AddForce(car.model.forward * 5000 * m * Time.deltaTime);

        //model.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
        isBoosting = true;
        timeBeforeRechargeCurrent = timeBeforeRecharge;
    }
    public void BoostRestock() {

        if (timeBeforeRechargeCurrent > 0) { // boost recharge is cooling down
            timeBeforeRechargeCurrent -= Time.deltaTime;
            return;
        }

        if (secondsOfBoostCurrent >= secondsOfBoostTotal) {
            secondsOfBoostCurrent = secondsOfBoostTotal;
            return; // boost meter full, do nothing
        }

        secondsOfBoostCurrent += Time.deltaTime * rechargeSpeed;
    }
    void SetParticleRate(ParticleSystem p, float perSecond) {
        if (p == null) return;
        ParticleSystem.EmissionModule em = p.emission;
        em.rateOverTime = perSecond;
    }
}
