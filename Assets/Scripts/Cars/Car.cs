using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarBoosting))]
public class Car : MonoBehaviour {


    public float attackCost = 5;
    public bool infiniteFuel = false;
    public float maximumFuel = 100;
    public float currentFuel { get; private set; }

    public Weapon weapon { get; private set; }

    /// <summary>
    /// The ball that is the car. Think of it as a hamster ball.
    /// </summary>
    public Rigidbody ballBody { get; private set; }

    public CarBoosting boost { get; private set; }

    /// <summary>
    /// This is the orientation of the tires. -1 to 1
    /// -1 is left, and +1 is right.
    /// </summary>
    public float turnAmount { get; private set; }
    public float throttle { get; private set; }

    /// <summary>
    /// This script will snap the position's position to the ball,
    /// and orient the suspension to the ground.
    /// </summary>
    public Transform suspension;
    /// <summary>
    /// The model should be a child of suspension.
    /// It will turn on its local yaw to animate turning.
    /// </summary>
    public Transform model;

    public Transform aiSteerVisual;
    public TextMesh text;

    public ParticleSystem[] dustParticles;

    public float throttleMin = 800;
    public float throttleMax = 2000;

    public float maxSpeed = 200;
    private float health = 100;

    [HideInInspector] public Driver driver;
    public CarState state { get; private set; }

    

    void Start() {
        ballBody = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<Weapon>();
        boost = GetComponent<CarBoosting>();
        
        currentFuel = maximumFuel;
        SwitchState(new CarStateGround());
    }
    public void DrawRay(LineRenderer lineRenderer, Vector3 start, Vector3 end, Color color = default) {
        if (color == default) color = Color.black;

        lineRenderer.material.color = color;

        Vector3[] pts = { start, end };
        lineRenderer.SetPositions(pts);
    }
    public void InitSpeed(Car car) {
        if (car == null) return;
        if (car.ballBody == null) return;
        ballBody = GetComponent<Rigidbody>();
        ballBody.velocity = car.ballBody.velocity;
    }
    private void SwitchState(CarState newCS)
    {
        if (newCS == null) return;
        if (state != null) state.OnEnd();
        state = newCS;
        newCS.OnStart(this);
    }
    void FixedUpdate() {
        if (driver != null) driver.DriveFixedUpdate();
        SwitchState(state.Update()); // detect ground
        MoveCar();
        UpdateModel();
        ballBody.velocity += new Vector3(0, -10, 0) * Time.fixedDeltaTime;
    }
    void Update() {
        if (driver != null) driver.DriveUpdate(); // get input from player
        if (health <= 0) Destroy(gameObject);
    }
    public void Kill(bool killSilently = false) {
        health = 0;
        Destroy(gameObject);
    }
    void OnDestroy() {
        if (driver != null) driver.OnDestroy((health <= 0));
    }
    public void AddFuel(float delta)
    {
        currentFuel += delta;
        currentFuel = Mathf.Clamp(currentFuel, 0, maximumFuel);
    }
    public void Jump() {

        // check if grounded
        if (!state.isGrounded) return;

        ballBody.AddForce(Vector3.up * 20, ForceMode.Impulse);
    }
    /// <summary>
    /// This function should be called from Update(), not FixedUpdate(). So deltaTime should be used.
    /// </summary>
    public void Boost() {
        print("boost?");
        boost.Boost();
    }
    public void SetThrottle(float throttlePercent) {
        if (throttlePercent < 0) throttlePercent = 0;
        throttle = Mathf.Lerp(throttleMin, state.throttleMultiplier * throttleMax, throttlePercent);
        AddFuel(-throttlePercent * Time.deltaTime); // lose 1 fuel per second
    }
    public void Turn(float amount) {
        turnAmount = amount;
    }
    public void Turn(int amount) {
        turnAmount = amount;
    }
    public void FireWeapons() {
        if (weapon != null) weapon.FireWeapons();
    }
    public void SandParticles(bool onSand)
    {
        foreach (ParticleSystem p in dustParticles) {
            SetParticleRate(p, onSand ? 50 : 0);
        }

        float amt = 0;
        if (onSand) {
            float t = ballBody.velocity.sqrMagnitude / 10000;
            t *= t;
            amt = Mathf.Lerp(0, 100, t);
        }
        // DUST:
        if (amt > Random.Range(0, 100f)) {
            WorldParticles.obj.Emit(
                    EffectType.Dust,
                    transform.position,
                    ballBody.velocity);
        }

        // SAND:
        foreach (ParticleSystem p in dustParticles) {

            if (amt > Random.Range(0, 200f)) {
                float X = Random.Range(-2, 2);
                float Y = Random.Range(10, 40);
                float Z = Random.Range(0, 0);

                WorldParticles.obj.Emit(
                        EffectType.Sand,
                        p.transform.position,
                        ballBody.velocity * .99f + new Vector3(X, Y, Z));
            }
        }
    }
    void SetParticleRate(ParticleSystem p, float perSecond)
    {
        if (p == null) return;
        ParticleSystem.EmissionModule em = p.emission;
        em.rateOverTime = perSecond;
    }
    public void Hurt(float amount) {
        health -= amount;
        if(health <= 0) gameObject.SendMessage("Explode");
    }

    private void MoveCar() {
        if (currentFuel <= 0) return;
        
        // move forward:
        ballBody.AddForce(state.forward * throttle * state.throttleMultiplier * Time.deltaTime);

        // move horizontal:
        if (turnAmount == 0) {
            ballBody.velocity = DecelerateHorizontal(ballBody.velocity);
        } else {
            ballBody.velocity = AccelerateHorizontal(ballBody.velocity, turnAmount);
        }
    }
    private Vector3 AccelerateHorizontal(Vector3 velocity, float direction) {
        velocity.x += 100 * direction * state.turnMultiplier * Time.fixedDeltaTime;
        float maxHorizontalSpeed = 100;
        if (velocity.x > maxHorizontalSpeed) velocity.x = maxHorizontalSpeed;
        if (velocity.x < -maxHorizontalSpeed) velocity.x = -maxHorizontalSpeed;
        return velocity;
    }
    private Vector3 DecelerateHorizontal(Vector3 velocity) {
        velocity.x = MathStuff.Damp(velocity.x, .01f, Time.fixedDeltaTime);
        return velocity;
    }
    public void UpdateModel() {

        // rotate the suspension to align with provided rotation:
        suspension.position = transform.position; // make the model follow the hamster wheel! ////////////////////// NOTE: If suspension is a child of the veichle do we need thi? 
        suspension.rotation = Quaternion.RotateTowards(suspension.rotation, state.suspensionOrientation, state.suspensionRotateSpeed * Time.deltaTime);

        // rotate the car model to align with velocity:
        
        if (model) {
            Vector3 vel = ballBody.velocity;
            float turn = Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg;
            model.localEulerAngles = new Vector3(0, turn, 0);
        }
        
    }
}
