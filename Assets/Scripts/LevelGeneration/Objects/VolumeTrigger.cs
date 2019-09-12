using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a volume that a car can overlap with to trigger some behavior.
/// </summary>
public class VolumeTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider col) {
        Car car = col.GetComponentInParent<Car>();
        if(car) Trigger(car);
    }
    /// <summary>
    /// This method is called when a car enters the volume. The method should be overriden by subclasses.
    /// </summary>
    /// <param name="car">The car that just entered the volume.</param>
    public virtual void Trigger(Car car) { }
}
