using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAI : Driver {

    /// <summary>
    /// Distance from attack target at which to giveup and die. This is useful if
    /// the AI's car falls out of the map or gets stuck somehow.
    /// </summary>
    private const int KILL_DIS = 100;

    /// <summary>
    /// How far to shoot a raycast forward for obstacle avoidance.
    /// </summary>
    private const int LOOK_DIS = 50;

    /// <summary>
    /// We want to drive towards this thing
    /// </summary>
    private Vector3 steeringTarget;

    /// <summary>
    /// The vehicle this ai would like to attack
    /// </summary>
    public Car attackTarget;

    float turnAmount = 0;
    float throttleAmount = 1;

    override public void Drive() {
        car.infiniteFuel = true;

        FindAnAttackTarget(); // find nearest player
        DestroyIfTooFarAway(); // if too far away, destroy self

        AdjustThrottle(); // control the foot on the throttle

        // TODO: turn this into a state machine ??
        bool avoidingObstacles = SteerAvoidObstacles();
        if (!avoidingObstacles) {
            bool pathFound = SteerTowardsPath(); // steer towards nearest path
            SteerTowardsAttackTarget(pathFound);
            SteerTowardsTarget();
        }

        ApplySteeringAndThrottle();
    }

    private void AdjustThrottle() {
        if (attackTarget == null) return;

        Vector3 disToTarget = attackTarget.transform.position - car.transform.position;

        // adjust throttle by distance from player:
        throttleAmount = disToTarget.z / 1;
        throttleAmount = Mathf.Clamp(throttleAmount, 0, 1);

        // if far behind, boost:
        if (disToTarget.z > 10) car.Boost();

    }

    private void FindAnAttackTarget() {
        if (attackTarget == null) {
            DriverPlayer player = PlayerManager.FindNearest(car.transform.position);
            if (player != null) attackTarget = player.car;
        }
    }

    private void ApplySteeringAndThrottle() {
        car.SetThrottle(throttleAmount);
        car.Turn(turnAmount);
        turnAmount = 0;
    }

    //////////////////////////////////////////////////////////

    
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
        float distance = LOOK_DIS;

        Vector3 rayStart = car.transform.position;
        Vector3 rayEnd = car.transform.position + forward * distance;


        if (Physics.Raycast(rayStart, forward, out look, distance)) {

            bool colliderIsPlayer = look.collider.gameObject.CompareTag("Player");
            bool colliderIsPickup = look.collider.gameObject.CompareTag("Pickup");

            if (colliderIsPlayer) {

            } else if (colliderIsPickup) {

            } else {

                bool colliderIsRamp = look.normal.y > 0.5f;
                float normalX = look.normal.x;

                if (!colliderIsRamp) {

                    car.SetLine(rayStart, rayEnd, Color.magenta);

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
        car.SetLine(rayStart, rayEnd);
        return false;
    }
    
    bool SteerTowardsPath() {
        steeringTarget = DrivePath.ProjectToNearestPath(car.transform.position, out bool pathWasFound);
        return pathWasFound;        
    }
    public void SteerTowardsTarget() {
        float turnMultiplier = 10f;

        turnAmount = (steeringTarget.x - car.transform.position.x) * turnMultiplier;
        turnAmount = Mathf.Clamp(turnAmount, -1, 1);

        car.aiSteerVisual.position = steeringTarget;
        car.aiSteerVisual.rotation = Quaternion.identity;
    }

    private void SteerTowardsAttackTarget(bool steeringTowardsPath) {
        if (attackTarget == null) return;
        if (steeringTowardsPath) {
            Vector3 trgtPos = attackTarget.transform.position;
            Vector3 disToPath = steeringTarget - car.transform.position;
            Vector3 disToAttackTarget = attackTarget.transform.position - car.transform.position;

            bool pathIsCloser = (disToPath.sqrMagnitude < disToAttackTarget.sqrMagnitude);
            if (pathIsCloser) {
                Debug.Log("path is closer");
                return;
            }
        }
        bool targetIsLeftOfMe = car.transform.position.x > attackTarget.transform.position.x;
        float offset = targetIsLeftOfMe ? 5 : -5;
        steeringTarget = attackTarget.transform.position + new Vector3(offset, 0, 0);
    }

    public override void OnDestroy(bool isDead) {
        EnemySpawner.Remove(this);
    }
}