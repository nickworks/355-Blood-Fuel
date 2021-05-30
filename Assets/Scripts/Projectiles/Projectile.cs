using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    public Driver shooter { get; set; }

    public float lifespanAverage = 3;
    public float lifespanRange = 1;

    private float lifespanActual;
    public float age { get; private set; }

    public bool explodeInAir = false;
    public GameObject prefabExplosion;

    private void Awake() {
        float half = lifespanRange / 2;
        lifespanActual = lifespanAverage + Random.Range(-half, half);
    }
    public virtual void FixedUpdate() {
        age += Time.deltaTime;
        if (age >= lifespanActual) {
            if (explodeInAir) Explode();
            else Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other) {

        if (!CanIgnoreCollider(other)) Explode();
    }
    protected bool CanIgnoreCollider(Collider other) {
        if (shooter != null && shooter.car != null && shooter.car.gameObject == other.gameObject) {
            // This projectile overlapped with the thing that shot it.
            return true; // ignore it
        }
        if (other.gameObject.GetComponentInParent<Projectile>()) {
            // We ran into another homing projectile.
            return true; // ignore it
        }
        return false;
    }
    public void Explode() {
        Destroy(gameObject);
        GameObject particles = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(particles, 2);
    }

}
