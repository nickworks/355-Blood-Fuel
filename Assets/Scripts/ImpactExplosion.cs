using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactExplosion : MonoBehaviour {
    Vector3 prevVelocity;
    public float threshold = 0;
    public GameObject prefabExplosion;
    public bool ignoreVertical = true;
    public float spawnSafetyTimer = .1f;
    public TurretRotation turretRotation;
    void Start()
    {
    }
    public void Explode()
    {
        Destroy(gameObject);

        GameObject particles = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(particles, 2);
    }

    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag == undefined) return;
        if(col.gameObject.tag == "Pickup")
        {
            DriverPlayer.main.car.AddFuel(10);
            Destroy(col.gameObject);
        }
        else if(col.impulse.sqrMagnitude > threshold * threshold)
        {
            // dot product of impulse & back
            float multiplier = Vector3.Dot(col.impulse.normalized, Vector3.back);
            multiplier *= multiplier; // bend the curve

            if (col.impulse.sqrMagnitude * multiplier > threshold * threshold)
            {
                //Explode();// col.impulse.normalized);
                gameObject.SendMessage("Explode");
            }
        }
    }
}
