using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour {

    public GameObject prefabBarrel;
    public Transform spawnPoint;
    public float fuelPerBarrelTossed = 10;
    public Transform cursor;
    public AnimationCurve curve;
    float aimMaxDistance = 15;

    public Car car;

    void Start() {
        car = GetComponentInParent<Car>();
    }
    private void OnDestroy() {
        Destroy(cursor.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBarrel();
        }
    }
    void LateUpdate()
    {
        if(PlayerInput.mode == InputMode.Gamepad)
        {
            cursor.parent = car.suspension;
            AimWithAnalog();
        }
        if (PlayerInput.mode == InputMode.MouseKeyboard)
        {
            cursor.parent = null;
            AimWithMouse();
        }
        DrawAimPath();
    }
    void AimWithMouse()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // make a ray
        Plane aimPlane = new Plane(Vector3.up, transform.position); // make a plane

        float rayLength = 0;
        if (aimPlane.Raycast(ray, out rayLength)) // detect if the ray intersects the plane
        {
            Vector3 hit = ray.GetPoint(rayLength); // detect where the intersection is

            Vector3 dis = transform.position - hit;
            if (dis.sqrMagnitude > aimMaxDistance * aimMaxDistance)
            {
                dis = dis.normalized * aimMaxDistance;
                hit = transform.position - dis;
            }
            float yaw = -Mathf.Atan2(dis.z, dis.x) * 180 / Mathf.PI;
            float parentYaw = transform.parent.eulerAngles.y;
            transform.localEulerAngles = new Vector3(0, yaw - parentYaw, 0);
            cursor.position = hit;
        }
    }
    void AimWithAnalog()
    {
        float aimAxisH = Input.GetAxis("Horizontal2");
        float aimAxisV = Input.GetAxis("Vertical2");

        float trigger = Input.GetAxis("Triggers");

        //PlayerController.main.line.enabled = Input.GetButton("BumperRight");
        //PlayerController.main.line.enabled = (trigger < -.2f);

        Vector3 target = new Vector3(aimAxisH, 0, aimAxisV);
        if (target.sqrMagnitude > 1) target.Normalize();

        bool deadZone = (target.sqrMagnitude < .1f); // deadZone
        target *= aimMaxDistance;
        if (aimAxisV > 0) target.z *= 2;

        //DriverPlayer.main.line.enabled = !deadZone; // TEMP DISABLE

        if (deadZone) return;

        bool aimFurtherOut = (target.sqrMagnitude >= cursor.localPosition.sqrMagnitude);
        float inputAlignAmount = Vector3.Dot(target, cursor.localPosition);
        bool letsAimThisThing = (aimFurtherOut || inputAlignAmount < .5f || target.sqrMagnitude > .8f);
        if (letsAimThisThing)
        {
            cursor.localPosition += (target - cursor.localPosition) * Time.deltaTime * 4;
        }
        cursor.rotation = Quaternion.identity;

    }
    void DrawAimPath()
    {
        // TEMP DISABLE
        /*
        Vector3[] pts = new Vector3[DriverPlayer.main.line.positionCount];
        
        float height = 2;

        for (int i = 0; i < pts.Length; i++)
        {
            int max = pts.Length - 1 + 4;
            float p = i / (float)max;

            //p *= .6f;
            Vector3 pt = Vector3.Lerp(transform.position, cursor.position, p);
            pt.y += curve.Evaluate(p) * height;
            pts[i] = pt;
            if(i == 1)
            {
                spawnPoint.position = pt;
            }
        }

        DriverPlayer.main.line.SetPositions(pts);
        */
    }

    private void SpawnBarrel()
    {
        if (car.currentFuel > fuelPerBarrelTossed)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, Random.onUnitSphere);
            
            Vector3 dir = spawnPoint.position - transform.position;
            dir.Normalize();

            GameObject obj = Instantiate(prefabBarrel, transform.position + dir, rot);

            Rigidbody barrel = obj.GetComponent<Rigidbody>();
            barrel.velocity += car.ballBody.velocity; // inherit the car's velocity

            barrel.AddForce(dir * 13, ForceMode.Impulse); // push the barrel
            barrel.AddTorque(Random.onUnitSphere * 10); // random spin

            car.AddFuel(-fuelPerBarrelTossed); // lose fuel
        }
    }
}

