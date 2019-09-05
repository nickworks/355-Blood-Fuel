using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour {

    
    public float age = 0;
    public float lifespan = 3;

    public GameObject prefabExplosion;
    Rigidbody body;

    public AnimationCurve vertical;
    public Vector3 ptStart;
    public Vector3 ptEnd;

    void Start () {
        body = GetComponent<Rigidbody>();
	}
	void Update () {
        CountdownTimer();
    }

    private void CountdownTimer() {
        age += Time.deltaTime;
        if (age >= lifespan) {
            Explode();
        }
    }

    void Explode() {
        Destroy(gameObject);
        GameObject particles = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(particles, 2);
    }
}
