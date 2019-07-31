using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    public float speed;
    public int type;
    Rigidbody rb;
    // Use this for initialization
    void Start () {
       rb  = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
	}
}
