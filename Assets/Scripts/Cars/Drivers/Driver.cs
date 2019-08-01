using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Driver {

    public Car car { get; private set; }

    public void TakeControl(Car car) {
        this.car = car;
        this.car.driver = this; // if the car had a former driver? it's gone now
    }
    public abstract void OnDestroy();
    public abstract void Drive();
}
