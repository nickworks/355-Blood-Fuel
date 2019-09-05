using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : Weapon {

    public GameObject prefabBarrel;

    public Car car;

    void Start() {
        car = GetComponentInParent<Car>();
    }
    private void OnDestroy() {
        Destroy(cursor.gameObject); // remove cursor
    }
    public override void FireWeapons(Driver driver) {
        
        if (car.currentFuel < fuelPerBarrelTossed) return;
            
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, Random.onUnitSphere);

        Vector3 dir = GetArcAt(.1f) - GetArcAt(0);
        dir.Normalize();

        GameObject obj = Instantiate(prefabBarrel, spawnPoint.position, rot);

        ArcPhysics phys = obj.GetComponent<ArcPhysics>();
        phys.SetArc(driver, spawnPoint.position, car.ballBody.velocity, GetArc(10), 1);

        /*
        Rigidbody barrel = obj.GetComponent<Rigidbody>();
        barrel.velocity += car.ballBody.velocity; // inherit the car's velocity

        barrel.AddForce(dir * 13, ForceMode.Impulse); // push the barrel
        barrel.AddTorque(Random.onUnitSphere * 10); // random spin
        */

        //car.AddFuel(-fuelPerBarrelTossed); // lose fuel   
    }
}

