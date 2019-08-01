using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAI : Driver
{

    /// <summary>
    /// We want to drive towards this thing
    /// </summary>
    private Vector3 steeringTarget;

    /// <summary>
    /// The vehicle this ai would like to attack
    /// </summary>
    public Car attackTarget;

    override public void Drive()
    {
        car.infiniteFuel = true;
        if(attackTarget == null) {
            DriverPlayer player = PlayerManager.FindNearest(car.transform.position);
            if (player != null) attackTarget = player.car;
        }
        CheckIfDead();
        SteerTowardsPath();
        ApplySteeringAndThrottle();

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
    
    ///////////////////////////////////////////////////////// HANDLE DEATH ///////////////////////////////////////////////////////////////////

    /// <summary>
    /// This function checks if we are violating any bounds that would cause us to be considered dead
    /// </summary>
    void CheckIfDead()
    {
        if(attackTarget == null) {
            Debug.Log("dying cause I don't have an attackTarget");
            car.health = 0;
            return;

        }
        Vector3 vectorToTarget = (attackTarget.transform.position - car.transform.position);
        float targetDisSqr = vectorToTarget.sqrMagnitude;
        if (targetDisSqr >  50 * 50)
        { // too far away

            Debug.Log($"dying cause I'm too far away from the thing I want to attack ({vectorToTarget})");
            car.health = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //SendMessage("Explode");
        }
    }
    void OnTriggerStay(Collider other)
    {
        CheckCollider(other);
    }
    void OnTriggerEnter(Collider other)
    {
        CheckCollider(other);
    }
    void CheckCollider(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            //explosion.Explode();
            //SendMessage("Explode");
        }
        if (other.gameObject.CompareTag("SteerAway"))
        {
            //SteerAwayFrom(other);
        }
    }
    void SteerAwayFrom(Collider c)
    {
        bool turnRight = car.transform.position.x > c.transform.position.x;
        turnAmount = turnRight ? 1 : -1;

    }
    void SteerTowardsPath()
    {
        steeringTarget = DrivePath.ProjectToNearestPath(car.transform.position);
        float turnMultiplier = 10f;

        turnAmount = (steeringTarget.x - car.transform.position.x) * turnMultiplier;
        turnAmount = Mathf.Clamp(turnAmount, -1, 1);

        car.aiSteerVisual.position = steeringTarget;
        car.aiSteerVisual.rotation = Quaternion.identity;
    }

    public override void OnDestroy() {
        EnemySpawner.Remove(this);
    }
}