using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumePickupFuel : VolumeTrigger {

    public float fuelAmount = 20;

    public AudioClip soundToPlay;

    public override void Trigger(Car car) {
        car.AddFuel(fuelAmount);
        AudioSource.PlayClipAtPoint(soundToPlay, car.transform.position);
        //soundToPlay
        
        Destroy(gameObject);
    }
}
