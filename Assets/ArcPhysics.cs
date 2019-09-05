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

    public void SetArc(Driver shooter, Vector3 origin, Vector3 velocity, Vector3[] arc, float totalTime) {
        this.shooter = shooter;
        this.origin = origin;
        this.velocityFromCar = velocity;
        this.arc = arc;
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
