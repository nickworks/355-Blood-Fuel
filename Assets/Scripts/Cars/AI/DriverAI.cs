using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAI : Driver
{

    /// <summary>
    /// We want to drive towards this thing
    /// </summary>
    public Transform target;

    override public void Drive()
    {
        car.infiniteFuel = true;
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

    bool isDead = false;
    float turnAmount;
    float chargePercent;
    
    ///////////////////////////////////////////////////////// HANDLE DEATH ///////////////////////////////////////////////////////////////////

    /// <summary>
    /// This function checks if we are violating any bounds that would cause us to be considered dead
    /// </summary>
    void CheckIfDead()
    {
        if (!target)
        {
            isDead = false;
        }
        else
        {
            float targetDisSqr = (car.transform.position - target.position).sqrMagnitude;
            if (car.transform.position.z < target.position.z && targetDisSqr > 50 * 50)
            { // too far behind
                isDead = true;
            }
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
        Vector3 nearestPoint = DrivePath.ProjectToNearestPath(car.transform.position);
        float turnMultiplier = 10f;
        turnAmount = (nearestPoint.x - car.transform.position.x) * turnMultiplier;
        turnAmount = Mathf.Clamp(turnAmount, -1, 1);

        //if (nearestPoint.x < transform.position.x) turnAmount = -1;
        //if (nearestPoint.x > transform.position.x) turnAmount = 1;

        car.aiSteerVisual.position = nearestPoint;
        car.aiSteerVisual.rotation = Quaternion.identity;
    }

    public override void OnDestroy() {
        EnemySpawner.Remove(this);
    }
}