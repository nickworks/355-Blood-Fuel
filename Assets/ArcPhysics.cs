using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcPhysics : MonoBehaviour
{
    Vector3 origin;

    Vector3 velocityFromCar;
    Vector3 velocityForRigidbody;

    float totalTime;
    Vector3[] arc;
    Driver shooter;
    Rigidbody body;

    float currentTime = 0;

    void Start() {
        body = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// This method initializes the arc that this object should follow.
    /// </summary>
    /// <param name="shooter">The Driver that shot this projectile. This will be stored and used to make sure that a Driver can't hit themselves.</param>
    /// <param name="spawnPoint">The world-space position of where the arc begins.</param>
    /// <param name="carVelocity">The car's velocity when this object spawns. This object needs to inherit this velocity.</param>
    /// <param name="arcPoints">An array of points that make up the arc.</param>
    /// <param name="totalTime">How long the object should take to follow the arc.</param>
    public void SetArc(Driver shooter, Vector3 spawnPoint, Vector3 carVelocity, Vector3[] arcPoints, float totalTime) {
        this.shooter = shooter;
        this.origin = spawnPoint;
        this.velocityFromCar = carVelocity;

        // TODO: does it make sense to encapsulate the calculation of these two values: (???)

        this.arc = arcPoints;
        this.totalTime = totalTime;
    }
    public Vector3 GetPositionAtTime(float time) {
        float p = time / totalTime;
        int index = (int)(p * (arc.Length - 1));

        Vector3 a = velocityFromCar * time + arc[index];
        Vector3 b = velocityFromCar * time + arc[index + 1];

        float percentAtA = index / (float)(arc.Length - 1);
        float percentAtB = (index + 1) / (float)(arc.Length - 1);

        float pSub = (p - percentAtA) / (percentAtB - percentAtA);

        Vector3 pos = Vector3.Lerp(a, b, pSub);
        return origin + pos;
    }
    // FixedUpdate is called once each time the physics engine updates
    void FixedUpdate() {
        currentTime += Time.fixedDeltaTime;
        if (currentTime < totalTime) {
            Vector3 nextPosition = GetPositionAtTime(currentTime);
            // calculate the local velocity of the barrel:
            velocityForRigidbody = (nextPosition - transform.position) / Time.fixedDeltaTime;
            transform.position = nextPosition;

        } else {
            Disable();
        }
    }
    void OnTriggerEnter(Collider other) {

        if (shooter.car != null && shooter.car.gameObject == other.gameObject) {
            // This projectile overlapped with the thing that shot it.
            // We should ignore that.
            return;
        }
        Disable();
    }
    void Disable() {
        enabled = false;
        Collider collider = GetComponent<Collider>();
        if(collider != null) collider.isTrigger = false;
        if(body != null) body.velocity = velocityForRigidbody;
    }
}
