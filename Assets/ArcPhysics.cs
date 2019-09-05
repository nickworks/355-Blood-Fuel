using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcPhysics : MonoBehaviour
{
    Vector3 origin;
    Vector3 velocity;
    float totalTime;
    Vector3[] arc;
    Driver shooter;
    Rigidbody body;

    float currentTime = 0;

    void Start() {
        body = GetComponent<Rigidbody>();
    }

    public void SetArc(Driver shooter, Vector3 origin, Vector3 velocity, Vector3[] arc, float totalTime) {
        this.shooter = shooter;
        this.origin = origin;
        this.velocity = velocity;
        this.arc = arc;
        this.totalTime = totalTime;
    }
    public Vector3 GetPositionAtTime(float time) {
        float p = time / totalTime;
        int index = (int)(p * (arc.Length - 1));

        Vector3 a = velocity * time + arc[index];
        Vector3 b = velocity * time + arc[index + 1];

        float percentAtA = index / (float)(arc.Length - 1);
        float percentAtB = (index + 1) / (float)(arc.Length - 1);

        float pSub = (p - percentAtA) / (percentAtB - percentAtA);

        Vector3 pos = Vector3.Lerp(a, b, pSub);
        return origin + pos;
    }
    // Update is called once per frame
    void FixedUpdate() {
        currentTime += Time.fixedDeltaTime;
        if (currentTime < totalTime) {
            Vector3 nextPosition = GetPositionAtTime(currentTime);

            //body.velocity = (nextPosition - transform.position) / Time.fixedDeltaTime;
            transform.position = nextPosition;

        } else {
            Disable();
        }
    }
    void OnTriggerEnter(Collider other) {

        if (shooter.car != null && shooter.car.gameObject == other.gameObject) {
            print("collided with shooter");
            return;
        }
        Disable();
    }
    void Disable() {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = false;
        enabled = false;
    }
}
