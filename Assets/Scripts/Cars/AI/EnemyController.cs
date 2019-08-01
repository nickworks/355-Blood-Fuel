using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    Rigidbody body;
    Rigidbody targetBody;
    Transform target; 
    public Transform suspension;
    public Transform model;

    public float throttleMin = 790;
    public float throttleMax = 2500;
    public float turnMultiplier = 1;

    ImpactExplosion explosion;

    float distToPlayer;

    //Vector3 vector3 = new Vector3(0, 0, 1);

    /// <summary>
    /// The target distance from the player along the z axis
    /// </summary>
    float offset = 6;
    /// <summary>
    /// How long should we coast for in seconds
    /// </summary>
    float coastTimer;
    /// <summary>
    /// Are we touching the ground
    /// </summary>
    bool isGrounded = false;



    bool isDead = false;

    float chargePercent;


    enum AIStates {
        chase,
        coast,
        cutoff,
        charge,
        dash,
        fallback
        //cut player off?
        //set player into tailspin?
        //knock player of edge or into wall?
        // Cut Off: If we are in front of the player, move in their direction along the X-Axis
        // Suicide Dash/Push POC: If we are close to the player and moveing at a similar velocity, move twoard them along the X-Axis 
        // Charge, slow down for a second and then throw Themselves at the player



    }

    AIStates state = AIStates.chase;


    void Start() {
        body = GetComponent<Rigidbody>();

        body.AddForce(0, 0, 2000);
        explosion = GetComponent<ImpactExplosion>();
    }

    void Update() {
        HandleDead();
        if (!target) return; // fixes bug when player has been destroyed

        distToPlayer = Vector3.Distance(transform.position, target.position);

        RaycastHit hit;
        isGrounded = (Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f));

        Vector3 forward;

        if (isGrounded) {
            forward = Vector3.Cross(Vector3.right, hit.normal);
        } else {
            forward = Vector3.forward;
        }


        switch (state) {
            case AIStates.chase:
                Chase(forward);
                state = CheckExitChase();
                break;
            case AIStates.coast:
                Coast(forward);
                state = CheckExitCoast();
                break;
            case AIStates.cutoff:
                Cutoff(forward);
                state = CheckExitCutoff();
                break;
            case AIStates.charge:
                Charge(forward);
                state = CheckExitCharge();
                break;
            default:
                print("Error: AI Statemachine in EnemyController.cs is out of bounds");
                break;

        }

        CheckIfDead();

        HandleGroundedRotBehavior(hit, CalcYaw());
    }

    float AvoidObstacles(Vector3 forward) {
        RaycastHit look;
        if (Physics.Raycast(transform.position, forward, out look, body.velocity.z)) {
            if (!look.collider.gameObject.CompareTag("Player") && !look.collider.gameObject.CompareTag("Pickup")) {
                if (look.normal.y == 0 && look.normal.z < 0 && Mathf.Abs(look.normal.x) < .5f) {
                    float rightEdge = look.collider.bounds.max.x;
                    float leftEdge = look.collider.bounds.min.x;
                    if (Mathf.Abs(rightEdge - transform.position.x) > Mathf.Abs(leftEdge - transform.position.x)) {
                        return -1;
                    } else {
                        return 1;
                    }
                }
            }
        }
       return 0;
    }

    
    ////////////////////////////////////////////// Chase

    /// <summary>
    /// This behavior should be called when the enemy to catch up to the player or slow down to get close the player, it throttles baised on th eposition of the player
    /// </summary>
    /// <param name="forward"> The forward vector along whitch w should be adding our force </param>
    void Chase(Vector3 forward) {
        //print("chase");
        forward.x += AvoidObstacles( forward);

        float dist = target.position.z - transform.position.z; //how far away is the player

        float v = dist / offset;// divide that by the desierd offset

        float throttle = Mathf.Lerp(throttleMin, throttleMax, v);

        Vector3 force = forward * throttle;//forward speed

        body.AddForce(force * Time.deltaTime);
        //print("Enemy: " + body.velocity);
        //print(Vector3.Distance(transform.position,target.position));
        //print(body.velocity.z - targetBody.velocity.z);

    }

    /// <summary>
    /// This function contains the conditons under whitch we should exit the chase state 
    /// </summary>
    /// <returns>The state we should transition too, returnes AIStates.chase if no transition should take place</returns>
    AIStates CheckExitChase() {
        //if we are close to the player and not going a lot faster than them
        if (distToPlayer <= offset) {
            //print("close enough");
            if (body.velocity.z - targetBody.velocity.z < 10) {
                //print("correct velocity");
                EnterCoast();
                return AIStates.coast;
            }
        }
        return AIStates.chase;
    }

    /////////////////////////////////////////////////////////////////Coast

    /// <summary>
    /// This runes once before we enter the coast state
    /// </summary>
    void EnterCoast() {
        if (transform.position.z > target.position.z) {
            coastTimer = Random.Range(1f, 2);
        } else {
            coastTimer = Random.Range(3, 5);
        }
    }

    /// <summary>
    /// This behavior should be called when we want to coast alongside the player, it throttals baised on velocity
    /// </summary>
    /// <param name="forward">The forward vector along whitch w should be adding our force </param>
    void Coast(Vector3 forward) {
        // print("coast");
        forward.x += AvoidObstacles(forward) * 2;

        float v = targetBody.velocity.z - body.velocity.z ;
        //print(v);

        float throttle = Mathf.Lerp(throttleMin, throttleMax, v);

        Vector3 force = forward * throttle;//forward speed

        body.AddForce(force * Time.deltaTime);

        coastTimer -= Time.deltaTime;
    }

    /// <summary>
    /// This function contains the conditons under whitch we should exit the coast state 
    /// </summary>
    /// <returns>The state we should transition too, returnes AIStates.coast if no transition should take place</returns>
    AIStates CheckExitCoast() {
        if (distToPlayer >= offset) {
            return AIStates.chase;
        }
        
        if (coastTimer <= 0 && transform.position.z > target.position.z) {
            return AIStates.cutoff;
        } else if (coastTimer <= 0 && transform.position.z < target.position.z) {
            float v = Random.value;
            if (true){ //v <= 0.5f) {
                EnterCharge();
                return AIStates.charge;
            } else {
                return AIStates.dash;
            }
        }
        
        return AIStates.coast;
    }


    //////////////////////////////////////////////////////////////Cutoff

    /// <summary>
    /// Thsi function contatins the cutoff state behavior
    /// </summary>
    /// <param name="forward"></param>
    void Cutoff(Vector3 forward) {
        //print("cutoff");
        if (isGrounded) {
            bool withinRange = Mathf.Abs(transform.position.x - target.position.x) <= 2;

            if (transform.position.x > target.position.x && !withinRange) {
                forward.x += -turnMultiplier;
            } else if (!withinRange) {
                forward.x += turnMultiplier;
            }

            Vector3 force;

            if (withinRange) {
                force = forward;
            } else {
                float v = targetBody.velocity.z - body.velocity.z;
                float throttle = Mathf.Lerp(throttleMin, throttleMax, v);
                force = forward * throttle; // forward speed
            }

            body.AddForce(force * Time.deltaTime);
        } else {
            Coast(forward);
        }
    }

    AIStates CheckExitCutoff() {
        if (distToPlayer >= offset || transform.position.z < target.position.z) {
            return AIStates.chase;
        }
        return AIStates.cutoff;
    }


    ////////////////////////////////////////////////////////// Charge

    /// <summary>
    /// 
    /// </summary>
    void EnterCharge() {
        //transform.LookAt(target);
    }

    void Charge(Vector3 forward) {
       // print("charge");

        if (isGrounded && target.position.y - transform.position.y <= 5) {
            Vector3 direction = target.position - transform.position;

            float v = Mathf.Clamp(targetBody.velocity.z - body.velocity.z, 0, 1);

            float chargeMin = 25;
            float chargeMax = 50;

            float throttle = Mathf.Lerp(chargeMin, chargeMax, v);

            body.AddForce(direction * throttle);
        }

    }

    AIStates CheckExitCharge() {

        if (distToPlayer <= 3 || distToPlayer > offset) {
            //chargePercent = 0;
            return AIStates.chase;
        }
        return AIStates.charge;
    }

   


    /*
    void Dash(Vector3 forward) {
        float dist = target.position.z - transform.position.z; //how far away is the player

        float v = dist / offset;// divide that by the desierd offset

        float throttle = Mathf.Lerp(throttleMin, throttleMax, v);

        Vector3 force = forward * throttle;//forward speed

        body.AddForce(force * Time.deltaTime);
    }

    AIStates CheckExitDash() {
        return AIStates.dash;
    }

    void FallBack() {


    }

    AIStates CheckExitFallBack() {
        return AIStates.fallback;
    }
    */


    ///////////////////////////////////////////////////////// HANDLE DEATH ///////////////////////////////////////////////////////////////////

    /// <summary>
    /// This function checks if we are violating any bounds that would cause us to be considered dead
    /// </summary>
    void CheckIfDead() {

        if (transform.position.z < target.position.z && distToPlayer > 50) { // too far behind
            isDead = true;
        }
    }

    void HandleDead() {
        if (isDead || !target) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            //explosion.Explode();
            SendMessage("Explode");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Explosion")) {
            //explosion.Explode();
            SendMessage("Explode");
        }
    }


    //////////////////////////////////////////////////////// MODEL RNDERING //////////////////////////////////////////////////////////////////

    /// <summary>
    /// This function sets diffred rotation veluus depending on whether or not we are grounded 
    /// </summary>
    /// <param name="hit">The raycast thet detectrs the ground, used to get the normals</param>
    /// <param name="yaw">our yaw baised on the forward velocity</param>
    void HandleGroundedRotBehavior(RaycastHit hit, float yaw) {

        Quaternion rot;

        if (isGrounded) {
            rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
        } else {
            float pitch = -body.velocity.y * 2;
            rot = Quaternion.Euler(pitch, 0, 0);

        }

        SetModelPosAndRot(rot, yaw);
    }


    /// <summary>
    /// This function calculates the direction we are moving and converts that to a rotation value
    /// </summary>
    /// <returns>Our yaw baised on our forward velocity</returns>
    float CalcYaw() {
        float turnAngle = Mathf.Atan2(body.velocity.x, body.velocity.z) * 180 / Mathf.PI;
        return turnAngle;
    }


    /// <summary>
    /// This function positions and rotates the rendered mesh, it is called in HandleGroundedRotBehavior()
    /// </summary>
    /// <param name="rot">The rotation we want to apply to the mesh</param>
    /// <param name="turn">The curent yaw according to our velocity</param>
    void SetModelPosAndRot(Quaternion rot, float turn) {
        float rotateSpeed = isGrounded ? 120 : 40; // the maximum number of degrees to rotate per second

        suspension.position = transform.position; // make the model follow the hamster wheel! 
        suspension.rotation = Quaternion.RotateTowards(suspension.rotation, rot, rotateSpeed * Time.deltaTime);
        if (model) model.localEulerAngles = new Vector3(0, turn, 0);
    }

    void Explode()
    {
        //PlayerManager.playerOne.score += 500;
    }
}
