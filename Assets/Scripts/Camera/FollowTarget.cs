using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    public float targetDistance = 20;

    Camera cam;
	
	void Start () {
        cam = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (target) {
            transform.position = target.position;
        }
        if (cam) { // zoom camera in or out:
            cam.transform.localPosition += (new Vector3(0, 0, -targetDistance) - cam.transform.localPosition) * Time.deltaTime;
        }
	}
}
