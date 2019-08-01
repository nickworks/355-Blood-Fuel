using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAI : Driver {

    private const float KILL_DIS = 100;

    /// <summary>
    /// We want to drive towards this thing
    /// </summary>
    private Vector3 steeringTarget;

    /// <summary>
    /// The vehicle this ai would like to attack
    /// </summary>
    public Car attackTarget;

    override public void Drive() {
        car.infiniteFuel = true;

        FindAnAttackTarget(); // find nearest player
        DestroyIfTooFarAway(); // if too far away, destroy self
        bool avoidingObstacles = SteerAvoidObstacles();
        if(!avoidingObstacles) SteerTowardsPath(); // steer towards nearest path
        ApplySteeringAndThrottle();
    }

    private void FindAnAttackTarget() {
        if (attackTarget == null) {
            DriverPlayer player = PlayerManager.FindNearest(car.transform.position);
            if (player != null) attackTarget = player.car;
        }
    }

    private void ApplySteeringAndThrottle()
    {
        float h = turnAmount;
        float v = 1; // full throttle!
        car.SetThrottle(v);
        car.Turn(h);
        turnAmount = 0;
    }

    //////////////////////////////////////////////////////////

    ImpactExplosion explosion;

    float distToPlayer;

    /// <summary>
    /// The target distance from the player along the z axis
    /// </summary>
    float offset = 6;
    /// <summary>
    /// How long should we coast - in seconds
    /// </summary>
    float coastTimer;
    /// <summary>
    /// Are we touching the ground
    /// </summary>

    float turnAmount;
    float chargePercent;
    
    /// <summary>
    /// Checks if this driver is too far from its attack target.
    /// If it is, destroy this car.
    /// </summary>
    void DestroyIfTooFarAway() {
        if(attackTarget == null) {
            Debug.Log("dying cause I don't have an attackTarget");
            car.Kill(true);
            return;
        }
        Vector3 vectorToTarget = (attackTarget.transform.position - car.transform.position);
        float targetDisSqr = vectorToTarget.sqrMagnitude;
        if (targetDisSqr > KILL_DIS * KILL_DIS) {
            // too far away
            //Debug.Log($"dying cause I'm too far away from the thing I want to attack ({vectorToTarget})");
            car.Kill(true);
        }
    }
    bool SteerAvoidObstacles() {
        RaycastHit look;

        Vector3 forward = car.ballBody.velocity.normalized;
        float distance = car.ballBody.velocity.z;

        if (Physics.Raycast(car.transform.position, forward, out look, distance)) {

            bool colliderIsPlayer = look.collider.gameObject.CompareTag("Player");
            bool colliderIsPickup = look.collider.gameObject.CompareTag("Pickup");

            if (colliderIsPlayer) {

            } else if (colliderIsPickup) {

            } else {

                bool colliderIsRamp = look.normal.y < 0.5f;
                float normalX = look.normal.x;

                if (!colliderIsRamp) {

                    float rightEdge = look.collider.bounds.max.x;
                    float leftEdge = look.collider.bounds.min.x;

                    float disLeft = Mathf.Abs(leftEdge - car.transform.position.x);
                    float disRight = Mathf.Abs(rightEdge - car.transform.position.x);

                    if (disLeft < disRight) turnAmount = -1;
                    if (disRight < disLeft) turnAmount = 1;

                    return true;
                }
            }
        }
        return false;
    }
    void SteerTowardsPath() {
        steeringTarget = DrivePath.ProjectToNearestPath(car.transform.position);
        float turnMultiplier = 10f;

        turnAmount = (steeringTarget.x - car.transform.position.x) * turnMultiplier;
        turnAmount = Mathf.Clamp(turnAmount, -1, 1);

        car.aiSteerVisual.position = steeringTarget;
        car.aiSteerVisual.rotation = Quaternion.identity;
    }

    public override void OnDestroy(bool isDead) {
        EnemySpawner.Remove(this);
    }
}