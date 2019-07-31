using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAI : MonoBehaviour
{

    /// <summary>
    /// The car that this AI is driving.
    /// </summary>
    public Car car { get; private set; }

    public Transform driveTarget;

    /// <summary>
    /// We want to drive towards this thing
    /// </summary>
    public Transform target;

    void Start()
    {
        car = GetComponent<Car>();
        car.infiniteFuel = true;
        //body.AddForce(0, 0, 2000);
        explosion = GetComponent<ImpactExplosion>();
    }
    void OnDestroy()
    {
        EnemySpawner.EnemyDead();
    }
    void Update()
    {
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
            float targetDisSqr = (transform.position - target.position).sqrMagnitude;
            if (transform.position.z < target.position.z && targetDisSqr > 50 * 50)
            { // too far behind
                isDead = true;
            }
        }
        
    }

    void HandleDead()
    {
        if (isDead || !target)
        {
            Destroy(gameObject);
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
            SendMessage("Explode");
        }
        if (other.gameObject.CompareTag("SteerAway"))
        {
            //SteerAwayFrom(other);
        }
    }
    void SteerAwayFrom(Collider c)
    {
        bool turnRight = transform.position.x > c.transform.position.x;
        turnAmount = turnRight ? 1 : -1;

    }
    void SteerTowardsPath()
    {
        Vector3 nearestPoint = DrivePath.ProjectToNearestPath(transform.position);
        float turnMultiplier = 10f;
        turnAmount = (nearestPoint.x - transform.position.x) * turnMultiplier;
        turnAmount = Mathf.Clamp(turnAmount, -1, 1);

        //if (nearestPoint.x < transform.position.x) turnAmount = -1;
        //if (nearestPoint.x > transform.position.x) turnAmount = 1;
        this.driveTarget.position = nearestPoint;
        this.driveTarget.rotation = Quaternion.identity;
    }
    void Explode()
    {
        DriverPlayer.score += 500;
    }

}