using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [HideInInspector] public Car car;

    public float fuelPerShot = 10;
    public Transform spawnPoint; // this is a reference to where the projectiles would spawn
    public Transform cursor; // this is a reference to where the weapon is aiming
    
    float aimMaxDistance = 15;

    void Start() {
        car = GetComponentInParent<Car>();
    }
    private void OnDestroy() {
        Destroy(cursor.gameObject); // remove cursor
    }
    public virtual void AimAt(Vector3 pos) {

        // clamp to a max distance:
        Vector3 dis = pos - transform.position;
        float maxLateralDistance = 20;
        float disMag = dis.magnitude;
        if(disMag > maxLateralDistance) {
            dis *= maxLateralDistance / disMag;
        }

        cursor.position = transform.position + dis;
    }
    
    public abstract void FireWeapons();

}
