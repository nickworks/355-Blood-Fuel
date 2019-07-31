using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverPlayer : MonoBehaviour
{

    static public DriverPlayer main;
    static public float score = 0;

    public Car car { get; private set; }
    public LineRenderer line { get; private set; }

    public PlayerHUD prefabHUD;
    static PlayerHUD hud;

    void Start()
    {
        score = 0;
        main = this;
        car = GetComponent<Car>();
        line = GetComponentInChildren<LineRenderer>();
        if (!hud)
        {
            hud = Instantiate(prefabHUD);
            hud.SetCar(car);
        }
    }
    void OnDestroy()
    {
        if (hud) Destroy(hud.gameObject);
    }
    void Update()
    {
        //AddFuel(-1 * Time.deltaTime); // lose 1 fuel per second
        if (car.currentFuel > 1) score += Time.deltaTime;

        float h = Input.GetAxis("Horizontal");
        float t = Input.GetAxis("Triggers");

        float v = Input.GetAxis("Vertical");

        car.SetThrottle(Mathf.Max(-t, v));
        car.Turn(h);

        if (Input.GetButtonDown("Fire1")) car.Jump();
        if (Input.GetButtonDown("Fire2")) car.Boost();

    }
}
