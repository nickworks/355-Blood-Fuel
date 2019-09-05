using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelLauncher : Weapon {

    public GameObject prefabBarrel;

    public override void FireWeapons() {

        if (car.currentFuel < fuelPerShot) return;
        //car.AddFuel(-fuelPerBarrelTossed); // lose fuel
        ShootProjectile(prefabBarrel);
    }
}

