using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Driver {

    public Car car { get; private set; }
    private float age = 0;
    private float waveAmp;
    private float waveMedian;
    private float countdownWiggle = 0;

    public float steerOffset { get; private set; }
    public bool wantsToAim { get; protected set; }

    public void TakeControl(Car car) {
        this.car = car;
        this.car.driver = this; // if the car had a former driver? it's gone now
    }
    public abstract void OnDestroy(bool isDead);

    /// <summary>
    /// This function should be called from car.FixedUpdate().
    /// It's intended to update in sync with the physics engine.
    /// Raycasting and other physics stuff should be handled here.
    /// </summary>
    public abstract void DriveFixedUpdate();
    /// <summary>
    /// This function should be called from car.Update().
    /// It's intended to run every game tick.
    /// Player input (button presses) should be detected and handled here.
    /// </summary>
    public virtual void DriveUpdate() {
        age += Time.deltaTime;
        countdownWiggle -= Time.deltaTime;
        if(countdownWiggle <= 0) {
            waveAmp = Random.Range(1, 5f);
            waveMedian = Random.Range(10, 20f);
            countdownWiggle = Random.Range(3, 10f);
        }
        steerOffset = Mathf.Sin(age) * waveAmp + waveMedian;
    }
}
