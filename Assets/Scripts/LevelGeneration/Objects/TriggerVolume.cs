using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a volume that a car can overlap with to trigger some behavior.
/// </summary>
public class TriggerVolume : MonoBehaviour {

    // TODO: make subclasses of TriggerVolume

    public float fuelAmount = 10;

    void OnTriggerEnter(Collider col) {
        Car car = col.GetComponentInParent<Car>();
        if(car) Trigger(car);
    }
    public void Trigger(Car car) {
        car.AddFuel(fuelAmount);
        Destroy(gameObject);
    }
}
