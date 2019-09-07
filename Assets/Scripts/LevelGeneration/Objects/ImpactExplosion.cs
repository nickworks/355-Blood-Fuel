using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If this object hits something really fast, it blows up
/// </summary>
public class ImpactExplosion : MonoBehaviour {
    Vector3 prevVelocity;
    public float threshold = 0;
    public GameObject prefabExplosion;
    public bool ignoreVertical = true;
    public float spawnSafetyTimer = .1f;
    void Start()
    {
    }
    public void Explode()
    {
        Car car = GetComponent<Car>();
        if (car) {
            car.Kill(); // properly Destroys car / driver
        } else {
            Destroy(gameObject);
        }

        GameObject particles = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(particles, 2);
    }

    void OnCollisionEnter(Collision col)
    {
        
        // todo: remove this class ... maybe?

        if (col.gameObject.tag == "Pickup") return; // if we hit a pickup, ignore it
        
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
