using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcingProjectile : Projectile
{
    Vector3 origin;

    Vector3 velocityFromCar;
    Vector3 velocityForRigidbody;

    float totalTime;
    Vector3[] arc;

    Rigidbody body;
    bool isArcing = true;

    void Start() {
        body = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// This method initializes the arc that this object should follow.
    /// </summary>
    /// <param name="shooter">The Driver that shot this projectile. This will be stored and used to make sure that a Driver can't hit themselves.</param>
    /// <param name="arcPoints">An array of points that make up the arc.</param>
    public void SetArc(Driver shooter, Vector3[] arcPoints) {
        this.shooter = shooter;

        this.origin = this.transform.position;
        this.velocityFromCar = shooter.car.ballBody.velocity;

        // TODO: does it make sense to encapsulate the calculation of these two values: (???)

        this.arc = arcPoints;
        this.totalTime = 0.5f; // the arc should take about .5 seconds
    }
    private Vector3 GetPositionAtTime(float time) {
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
    // step the physics forward
    public override void FixedUpdate() {
        base.FixedUpdate();
        if (isArcing) {
            if (age < totalTime) {
                Vector3 nextPosition = GetPositionAtTime(age);
                // calculate the local velocity of the barrel:
                velocityForRigidbody = (nextPosition - transform.position) / Time.fixedDeltaTime;
                transform.position = nextPosition;

            } else {
                SwitchToPhysics();
            }
        }
    }
    void OnTriggerEnter(Collider other) {

        if (!CanIgnoreCollider(other)) SwitchToPhysics(); // if hit something, turn off arc motion
        
    }
    /// <summary>
    /// Turn off arcing and enable physics:
    /// </summary>
    void SwitchToPhysics() {
        isArcing = false;
        //enabled = false;
        Collider collider = GetComponent<Collider>();
        if(collider != null) collider.isTrigger = false; // turn on physics!
        if(body != null) body.velocity = velocityForRigidbody; // add velocity!
    }
}
