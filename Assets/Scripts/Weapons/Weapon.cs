﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [HideInInspector] public Car car;

    Vector3 aimOffset = Vector3.zero;

    public bool isFullAuto = false;
    public float roundsPerSecond = 2;
    public float baseDelayBetweenRounds {
        get {
            if (roundsPerSecond <= 0) return 1;
            return 1/roundsPerSecond;
        }
    }
    float cooldownShot;

    public float fuelPerShot = 10;

    public Transform spawnPoint; // this is a reference to where the projectiles would spawn
    public Transform cursor; // this is a reference to where the weapon is aiming
    
    float aimMaxDistance = 15;


    void Start() {
        car = GetComponentInParent<Car>();
    }
    public virtual void Update() {
        if (cooldownShot > 0) cooldownShot -= Time.deltaTime;
    }
    private void FixedUpdate() {
        cursor.position = transform.position + aimOffset;
    }
    private void OnDestroy() {
        Destroy(cursor.gameObject); // remove cursor
    }
    public virtual void AimAt(Vector3 pos) {

        // clamp to a max distance:
        Vector3 dis = pos - transform.position;
        float maxLateralDistance = 10;
        float disMag = dis.magnitude;
        if(disMag > maxLateralDistance) {
            //dis *= maxLateralDistance / disMag;
        }
        aimOffset = dis;
    }
    
    public virtual bool FireWeapons() {
        if (cooldownShot > 0) return false;
        if (car.currentFuel < fuelPerShot) return false;
        cooldownShot = baseDelayBetweenRounds;
        return true;
    }

}
