using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumePickupFuel : VolumeTrigger {

    public float fuelAmount = 20;

    public override void Trigger(Car car) {
        car.AddFuel(fuelAmount);
        Destroy(gameObject);
    }
}
