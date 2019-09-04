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
    public override void FireWeapons() {
        SpawnBarrel();
    }

    private void SpawnBarrel() {
        if (car.currentFuel > fuelPerBarrelTossed) {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, Random.onUnitSphere);
            
            Vector3 dir = cursor.position - spawnPoint.position;
            dir.Normalize();

            GameObject obj = Instantiate(prefabBarrel, transform.position + dir, rot);

            Rigidbody barrel = obj.GetComponent<Rigidbody>();
            barrel.velocity += car.ballBody.velocity; // inherit the car's velocity

            barrel.AddForce(dir * 13, ForceMode.Impulse); // push the barrel
            barrel.AddTorque(Random.onUnitSphere * 10); // random spin

            car.AddFuel(-fuelPerBarrelTossed); // lose fuel
        }
    }
}

