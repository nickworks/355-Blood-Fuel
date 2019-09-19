using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class simply stores a reference to the driver that
/// shot this projectile. This prevents self-injury.
/// </summary>
public class ProjectileController : MonoBehaviour {

    Driver shooter;

    public void SetShooter(Driver driver) {
        shooter = driver;
    }
    public bool IsShooter(GameObject obj) {
        return (obj == shooter.car.gameObject);
    }
}
