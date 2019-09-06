using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Driver {

    public Car car { get; private set; }

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
    public virtual void DriveUpdate() { }
}
