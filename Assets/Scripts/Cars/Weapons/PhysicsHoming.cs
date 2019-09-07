using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHoming : MonoBehaviour
{
    float boostStrength = 50;
    float boostTime = 2;
    float boostWiggle = 10;
    [Tooltip("This graph blends the boost direction from initial launch direction to homing direction.")]
    public AnimationCurve blendFalloff;
    Vector3 launchDirection = Vector3.up;
    Transform target;
    Vector3 velocity;
    Vector3 origin;
    float age = 0;
    Driver shooter;
    
    void Start()
    {
        
    }
    public void Launch(Driver shooter, Vector3 origin, Vector3 dir, Transform target, Vector3 carVelocity) {

        launchDirection = dir + Random.insideUnitSphere * .5f;
        this.shooter = shooter;
        this.target = target;
        this.origin = origin;
        this.velocity = carVelocity;
    }
    // Update is called once per frame
    void Update() {


    }
    void FixedUpdate() {
        age += Time.fixedDeltaTime;
        if (target == null) {
            gameObject.SendMessage("Explode");
            return;
        }


        Vector3 homingDirection = (target.position - transform.position).normalized;

        Vector3 boostDirection = Vector3.Lerp(launchDirection, homingDirection, age);
        // set position to origin + velocity
        velocity += boostDirection * boostStrength * Time.deltaTime;
        transform.position += velocity * Time.fixedDeltaTime;

        transform.rotation = Quaternion.LookRotation(boostDirection);
    }
    void OnTriggerEnter(Collider other) {

        if (shooter.car != null && shooter.car.gameObject == other.gameObject) {
            // This projectile overlapped with the thing that shot it.
            return; // ignore it
        }
        if (other.gameObject.GetComponentInParent<PhysicsHoming>()) {
            // We ran into another homing projectile.
            return; // ignore it
        }
        gameObject.SendMessage("Explode");
    }
}
