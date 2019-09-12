using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeDamage : VolumeTrigger {

    public float damageAmount = 10;
    public float pushbackStrength = 10;

    public override void Trigger(Car car) {
        car.Hurt(damageAmount);
        Vector3 dir = (transform.position - car.ballBody.transform.position).normalized;
        car.ballBody.AddForce(dir * pushbackStrength, ForceMode.Impulse);
    }
}
