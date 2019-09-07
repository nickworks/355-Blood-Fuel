using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Car target;
    private float cameraDistance = 0;
    public float cameraDistanceMin = 1;
    public float cameraDistanceMax = 20;
    public AnimationCurve cameraDistanceCurve;

    private float cameraAngle = 90;
    private float cameraAngleMin = 90;
    private float cameraAngleMax = 120;
    private float cameraPitch = 0;
    public Transform pitchControl;

    Camera cam;
    public float maxSpeed = 200;

    void Start () {
        cam = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    void FixedUpdate() {

        if (target) {
            
            // move the camera-target towards the target car's position:
            float distanceAboveCar = 2;
            transform.position = MathStuff.Damp(transform.position, target.transform.position + new Vector3(0, distanceAboveCar, 0), 0f, Time.fixedDeltaTime);

            // calculate a distanceMultiplier from velocity
            float p = 1;
            if (target.ballBody) p = Mathf.Clamp(target.ballBody.velocity.z / maxSpeed, 0, 1);
            if (target.isBoosting) p = 1;
            p = Mathf.Clamp(p, 0, 1);
            p = cameraDistanceCurve.Evaluate(p);
            cameraDistance = Mathf.Lerp(cameraDistanceMin, cameraDistanceMax, p);

            cameraAngle = Mathf.Lerp(cameraAngleMin, cameraAngleMax, p);


            // CAMERA SHAKE:
            cameraPitch = 30;
            if (target.isBoosting) {
                float amp = 10 * target.ballBody.velocity.z / maxSpeed;
                cameraPitch += Random.Range(-amp, amp);
            }

        }
        if (cam) { // zoom camera in or out:

            Vector3 localPositionTarget = new Vector3(0, 0, -cameraDistance);
            cam.transform.localPosition = MathStuff.Damp(cam.transform.localPosition, localPositionTarget, .5f, Time.fixedDeltaTime);

            pitchControl.localRotation = MathStuff.Damp(pitchControl.localRotation, Quaternion.Euler(cameraPitch, 0, 0), .1f, Time.fixedDeltaTime);
            cam.fieldOfView = MathStuff.Damp(cam.fieldOfView, cameraAngle, .5f, Time.fixedDeltaTime);
        }
    }
}
