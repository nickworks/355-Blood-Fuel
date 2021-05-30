using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
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
    
    public void Launch(Driver shooter, Vector3 dir) {

        launchDirection = dir + Random.insideUnitSphere * .5f;
        this.shooter = shooter;
        this.target = shooter.car.weapon.cursor;
        this.origin = transform.position;
        this.velocity = shooter.car.ballBody.velocity;
    }
    override public void FixedUpdate() {
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
}
