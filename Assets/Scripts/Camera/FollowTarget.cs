using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Car target;
    public float targetDistance = 20;
    float distanceMultiplier = 1;

    Camera cam;
	
	void Start () {
        cam = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (target) {
            transform.position = target.transform.position;
            float modifier = 1;
            if (target.ballBody) modifier = (target.ballBody.velocity.z - 50) / 10;
            modifier = Mathf.Clamp(modifier, 0, 1);
            distanceMultiplier = 1 + (modifier * modifier) / 2;
        }
        if (cam) { // zoom camera in or out:

            Vector3 localPositionTarget = new Vector3(0, 0, -targetDistance * distanceMultiplier);

            cam.transform.localPosition += (localPositionTarget - cam.transform.localPosition) * Time.deltaTime;
        }
	}
}
